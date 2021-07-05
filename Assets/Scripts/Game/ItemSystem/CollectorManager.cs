using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Items;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class CollectorManager : MonoBehaviour
{
    #region Properties
    public static CollectorManager instance;
    [SerializeField] UnityEvent Pickup;
    [SerializeField]Interactions interactions;
    [SerializeField] AudioClip reserveAcquired, itemAcquired;
    [SerializeField] GameObject player,acquiredPanel,canvas;
    [SerializeField] Image suitUI;
    [SerializeField] Suit gravity,corrupt;
    [SerializeField] GameObject[] defaultAmmoPrefabs;
    [SerializeField] ButtonUtilities buttonEssentials;
    [SerializeField] AudioMixerGroup mixerToMute;

    private PlayerInventory inventory;
    private float audioAux;
    private SkinSwapper skin;
    private AudioSource audioPlayer;
    private PlayerController playerC;
    private GameObject itemGot,panel;

    public Dictionary<int, ReserveAcquired> reserveSearch { get; set; }
    public Dictionary<int, ItemAcquired> itemSearch { get; set; }
    #endregion
    #region Public Methods
    private void VerifyRegistry<T>(T behaviour)where T:ICollecteable{
        if(behaviour is ReserveAcquired){
            ReserveAcquired reserve=behaviour as ReserveAcquired;
            if(!reserveSearch.ContainsKey(reserve.ID)){
                reserveSearch.Add(reserve.ID, reserve);
                //HandlePickupReserve(reserve);
            }else Destroy(reserve.gameObject);
        }else if(behaviour is ItemAcquired){
            ItemAcquired item = behaviour as ItemAcquired;
            if(!itemSearch.ContainsKey(item.ID)){
                itemSearch.Add(item.ID, item);
                //HandlePickupItem(item);
            }
            else Destroy(item.gameObject);
        }
    }
    private void AddToPlayerInventory(ReserveAcquired reserve){
        inventory.reserve.Add(reserve.ID);
    }
    private void AddToPlayerInventory(ItemAcquired item){
        inventory.AddToItems(new Item(true, item.ID));
        buttonEssentials.SetButton(item.ID, true);
        interactions.SetButtonNavigation();
    }
    public void SetPause()
    {
        mixerToMute.audioMixer.GetFloat("SE volume",out audioAux);
        mixerToMute.audioMixer.SetFloat("SE volume", -80);
        Pause.PausePlayer(playerC,true);
    }
    #endregion
    #region Unity Methods
    private void Start()
    {
        instance=this;
        if(reserveSearch==null)reserveSearch = new Dictionary<int, ReserveAcquired>();
        if (itemSearch == null) itemSearch = new Dictionary<int, ItemAcquired>();
        GameEvents.verifyRegistry+=VerifyRegistry;
        playerC = player.GetComponent<PlayerController>();
        inventory = player.GetComponent<PlayerInventory>();
        audioPlayer = GetComponent<AudioSource>();
        //skin = player.GetComponent<SkinSwapper>();
    }
    private void OnDisable() {
        GameEvents.verifyRegistry-=VerifyRegistry;
    }
    #endregion
    #region Private Methods
     
    private void ReserveAcquired(ReserveAcquired reserve)
    {
        itemGot = reserve.gameObject;
        var ammo = inventory.limitedAmmo;
        switch (reserve.ItemType)
        {
            case ReserveType.Missile:
                ammo[0].AddCapacity(200);
                break;
            case ReserveType.SuperMissile:
                 //Check if is the first time on get the item.
                if (!inventory.CheckLimitedAmmo(1))
                {
                    CountableAmmo newAmmo =new CountableAmmo(false, 1, defaultAmmoPrefabs[0], 2, 2);
                    ammo[1]=newAmmo;
                    GameUI.enableUI.Invoke(1);
                    GameUI.ammoText.Invoke(1,2);
                }else{
                    ammo[1].AddCapacity(2);
                    GameUI.ammoText.Invoke(1, ammo[1].actualAmmo);
                }
                break;
            case ReserveType.SuperBomb:
                //Check if is the first time on get the item.
                if (!inventory.CheckLimitedAmmo(2))
                {
                    CountableAmmo newAmmo =new CountableAmmo(false, 2, defaultAmmoPrefabs[1], 2, 2); 
                    ammo[2]=newAmmo;
                    GameUI.enableUI.Invoke(2);
                    GameUI.ammoText.Invoke(2, 2);
                }else{
                    ammo[2].AddCapacity(2);
                    GameUI.ammoText.Invoke(2, ammo[2].actualAmmo);
                }
                break;
            case ReserveType.EnergyTank:
                GameEvents.healthTank.Invoke();
                break;
            case ReserveType.BouncingBomb:
                //Check if is the first time on get the item.
                if (!inventory.CheckLimitedAmmo(3))
                {
                    CountableAmmo newAmmo =new CountableAmmo(false, 3, defaultAmmoPrefabs[2], 10, 10);
                    ammo[3]=newAmmo;
                    GameUI.enableUI.Invoke(3);
                    GameUI.ammoText.Invoke(3, 10);
                }else{
                    ammo[3].AddCapacity(10);
                    GameUI.ammoText.Invoke(3, ammo[3].actualAmmo);
                }
            break;
        }
        AddToPlayerInventory(reserve);
        audioPlayer.ClipAndPlay(reserveAcquired);
        panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = reserve.nameItem;
        Pause.onAnyMenu = true;
    }
    private void ItemAcquired(ItemAcquired item)
    {
        if (item.iType == ItemType.Suit)
        {
            suitUI.sprite = gravity.portait;
            skin=player.GetComponent<SkinSwapper>();
            skin.SetGravitySuit();
        }
        itemGot = item.gameObject;
        string itemName = item.name;
        panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemName;
        panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.Message;
        AddToPlayerInventory(item);
        audioPlayer.ClipAndPlay(itemAcquired);
        Pause.onAnyMenu = true;
    }
    public void HandlePickupItem(ItemAcquired itemS)
    {
        Pickup.Invoke();
        ItemAcquired(itemS);
        StartCoroutine(Resume(2f));
    }
    public void HandlePickupReserve(ReserveAcquired reserveItem)
    {
        Pickup.Invoke();
        ReserveAcquired(reserveItem);
        StartCoroutine(Resume(reserveAcquired.length));
    }
    public void ShowAcquiredPanel(){
        panel=Instantiate(acquiredPanel,canvas.transform.position,Quaternion.identity,canvas.transform);
    }
    IEnumerator Resume(float audioLenght)
    {
        yield return new WaitForSecondsRealtime(audioLenght);
        Pause.UnpausePlayer(playerC);
        mixerToMute.audioMixer.SetFloat("SE volume",audioAux);
        Pause.onAnyMenu = false;
        Destroy(panel);Destroy(itemGot.gameObject);
    }
    #endregion
}
