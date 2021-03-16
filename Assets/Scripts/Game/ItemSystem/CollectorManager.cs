using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System;
using Items;
using UnityEngine.Audio;

public class CollectorManager : MonoBehaviour
{
    #region Properties
    [SerializeField] UnityEvent Pickup;
    [SerializeField]Interactions interactions;
    [SerializeField] AudioClip reserveAcquired, itemAcquired;
    [SerializeField] GameObject player,suitUI,acquiredPanel,canvas;
    [SerializeField] GameObject[] defaultAmmoPrefabs;
    [SerializeField] Sprites playerSuits;
    [SerializeField] GameUI hudUI;
    [SerializeField] ButtonUtilities buttonEssentials;
    [SerializeField] AudioMixerGroup mixerToMute;

    private PlayerInventory inventory;
    private float audioAux;
    private ChangeSkin skin;
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
                reserve.onPickup += HandlePickupReserve;
            }else Destroy(reserve.gameObject);
        }else if(behaviour is ItemAcquired){
            ItemAcquired item = behaviour as ItemAcquired;
            if(!itemSearch.ContainsKey(item.ID)){
                itemSearch.Add(item.ID, item);
                item.onPickup += HandlePickupItem;
            }
            else Destroy(item.gameObject);
        }
    }
    private void AddToPlayerInventory(ReserveAcquired reserve){
        inventory.reserve.Add(reserve.ID);
    }
    private void AddToPlayerInventory(ItemAcquired item){
        inventory.AddToItems(new PlayerInventory.Item(true, item.ID));
        buttonEssentials.SetButton(item.ID, true);
        interactions.SetButtonNavigation();
    }
    public void SetPause()
    {
        mixerToMute.audioMixer.GetFloat("SE volume",out audioAux);
        mixerToMute.audioMixer.SetFloat("SE volume", -80);
        Pause.PausePlayer(playerC);
    }
    #endregion
    #region Unity Methods
    private void Start()
    {
        if(reserveSearch==null)reserveSearch = new Dictionary<int, ReserveAcquired>();
        if (itemSearch == null) itemSearch = new Dictionary<int, ItemAcquired>();
        GameEvents.verifyRegistry+=VerifyRegistry;
        playerC = player.GetComponent<PlayerController>();
        inventory = player.GetComponent<PlayerInventory>();
        audioPlayer = GetComponent<AudioSource>();
        skin = player.GetComponent<ChangeSkin>();
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
        var ammoSearch = inventory.limitedAmmoSearch;
        switch (reserve.ItemType)
        {
            case ReserveType.Missile:
                ammo[0].AddCapacity(999);
                break;
            case ReserveType.SuperMissile:
            //Check if is the first time on get the item.
                if (!ammoSearch.ContainsKey(1))
                {
                    PlayerInventory.CountableAmmo newAmmo =
                        new PlayerInventory.CountableAmmo(false, 1, defaultAmmoPrefabs[0], 0, 0);
                    ammoSearch.Add(1, newAmmo);
                    ammo.Add(newAmmo);
                    hudUI.AddAndSubscribe(1);
                }
                ammoSearch[1].AddCapacity(999);
                break;
            case ReserveType.SuperBomb:
                //Check if is the first time on get the item.
                if (!ammoSearch.ContainsKey(2))
                {
                    PlayerInventory.CountableAmmo newAmmo =
                        new PlayerInventory.CountableAmmo(false, 2, defaultAmmoPrefabs[1], 0, 0);
                    ammoSearch.Add(2, newAmmo);
                    ammo.Add(newAmmo);
                    hudUI.AddAndSubscribe(2);
                }
                ammoSearch[2].AddCapacity(20);
                break;
            case ReserveType.EnergyTank:
                GameEvents.healthTank.Invoke();
                break;
        }
        AddToPlayerInventory(reserve);
        audioPlayer.ClipAndPlay(reserveAcquired);
        panel.transform.GetChild(0).GetComponent<Text>().text = reserve.nameItem;
        Pause.onAnyMenu = true;
    }
    private void ItemAcquired(ItemAcquired item)
    {
        if (item.iType == ItemType.Suit)
        {
            suitUI.GetComponent<Image>().sprite = playerSuits.sprite2;
            skin.SetGravitySuit();
        }
        itemGot = item.gameObject;
        string itemName = item.name;
        panel.transform.GetChild(0).GetComponent<Text>().text = itemName;
        panel.transform.GetChild(1).GetComponent<Text>().text = item.Message;
        AddToPlayerInventory(item);
        audioPlayer.ClipAndPlay(itemAcquired);
        Pause.onAnyMenu = true;
    }
    public void HandlePickupItem(ItemAcquired itemS)
    {
        Pickup.Invoke();
        ItemAcquired(itemS);
        itemS.onPickup -= HandlePickupItem;
        StartCoroutine(Resume(itemAcquired.length));
    }
    public void HandlePickupReserve(ReserveAcquired reserveItem)
    {
        Pickup.Invoke();
        ReserveAcquired(reserveItem);
        reserveItem.onPickup -= HandlePickupReserve;
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
