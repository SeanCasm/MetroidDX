using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Player;
using Items;
using Player.Weapon;

public class PlayerInventory : MonoBehaviour
{
    #region Encapsulated classes
    public class Inventory
    {
        public bool selected { get; set; }
        public int iD { get; set; }
        public Inventory(bool selected, int iD)
        {
            this.selected = selected;
            this.iD = iD;
        }
        public Inventory(){}
    }
    public class Ammo : Inventory
    {
        public GameObject ammoPrefab { get; set; }
        public static int ammoSelected;
        public Ammo(bool selected, int iD,GameObject ammoPrefab) : base(selected, iD)
        {
            this.ammoPrefab = ammoPrefab;
        }
    }
    public class CountableAmmo : Ammo
    {
        #region Properties
        public int maxAmmo { get; set; }
        public int actualAmmo { get; set; }
        public event Action<int, int> ammoText;
        public event Action<int, bool> toggleUI;
        public event Action<int> enableUI;
        #endregion
        public CountableAmmo(bool selected, int iD, GameObject ammoPrefab,
            int maxAmmo,int actualAmmo) : base(selected,  iD, ammoPrefab)
        {
            this.selected = selected; 
            this.iD = iD;this.ammoPrefab = ammoPrefab;
            this.maxAmmo = maxAmmo;this.actualAmmo = actualAmmo;
            GameEvents.refullAll += HandleRefullAll;
            
        }
        /// <summary>
        /// Creates the object after load the game.
        /// </summary>
        public CountableAmmo(bool selected, int iD, GameObject ammoPrefab,
            int maxAmmo) : base(selected, iD, ammoPrefab)
        {
            this.selected = false;
            this.maxAmmo = this.actualAmmo = maxAmmo;
            this.ammoPrefab = ammoPrefab;
            GameEvents.refullAll += HandleRefullAll;
        }
        #region Public methods
        public void StablishUI()
        {
            ammoText.Invoke(iD, actualAmmo);
            enableUI.Invoke(iD);
        }
        public void AddCapacity(int amount)
        {
            maxAmmo += amount; actualAmmo += amount;
            ammoText.Invoke(iD, maxAmmo);
        }
        public void ActualAmmoCount(int amount)
        {
            actualAmmo += amount;
            if (actualAmmo >= maxAmmo) actualAmmo = maxAmmo;
            else if (actualAmmo <= 0)
            {
                actualAmmo = 0; Select(false);
            }
            ammoText.Invoke(iD, actualAmmo);
        }
        public void Select(bool select)
        {
            toggleUI.Invoke(iD, select);
            this.selected = select;
            ammoSelected=iD;
        }
        public bool CheckAmmo()
        {
            if (actualAmmo > 0) return true;
            else return false;
        }
        /// <summary>
        /// Checks if actual ammo is lower than max ammo.
        /// </summary>
        /// <returns>true if actual ammo is lower tran max ammo, false if actual ammo is equals to maxAmmo</returns>
        public bool CheckCapacity(){
            if(actualAmmo<maxAmmo)return true;
            else return false;
        }
        private void HandleRefullAll()
        {
            this.actualAmmo=maxAmmo;
            ammoText.Invoke(iD, maxAmmo);
        }
        public void Unsubcribe(){
            GameEvents.refullAll-=HandleRefullAll;
        }
        #endregion
    }
    public class Item : Inventory
    {
        public static int[] beamsID { get; } = { 1, 2, 10 };
        public Beam beam{get;set;}
        public Item(bool selected, int iD) : base(selected, iD)
        {
            this.iD = iD;
            this.selected = selected;
        }
    }
    #endregion

    #region Properties
    [SerializeField] SkinSwapper playerSkin;
    [SerializeField]Interactions interactions;
    [SerializeField] Beams beams;
    [SerializeField] BaseData baseData;
    [SerializeField] ButtonUtilities buttonEssentials;
    [SerializeField]Pool pool;
    [SerializeField] PlayerInstantiates playerInstantiates;
    private GameData data;
    private PlayerController playerController;
    public Dictionary<int, Item> playerItems { get; set; }= new Dictionary<int, Item>();
    public List<int> reserve { get; set; }=new List<int>();
    public List<int> items { get; set; }=new List<int>();
    public bool canShootBeams { get ;set ; }=true;
    //0: missiles, 1: super missiles, 2: super bombs
    public List<CountableAmmo> limitedAmmo { get; set; }= new List<CountableAmmo>();
    public Dictionary<int, CountableAmmo> limitedAmmoSearch { get; set; } = new Dictionary<int, CountableAmmo>();
    #endregion
    #region Unity methods
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        baseData.SetInventoryData(this);
        buttonEssentials.SetButton(4, true);
        interactions.SetButtonNavigation();
        SetBeamToShoot();
    }
    private void OnEnable() {
        GameEvents.OnRetry -= OnRetry;
        GameEvents.OnRetry+=OnRetry;
    }
    void OnDisable()
    {
        foreach(var element in limitedAmmo){if(element!=null)element.Unsubcribe();}
        AmmoSelection();
    }
    #endregion
    #region Public methods
    public void AddToItems(Item item)
    {
        int id = item.iD;
        items.Add(id);
        playerItems.Add(id, item);
        if(id==9)SetGravityJump(id);
        else if(id==5)SetScrewJump(id);
        else if(id==7)ChangeJumpForce();//high jump

        foreach (int element in Item.beamsID)
        {
            if (id == element)DisableIncompatibleBeams(id);
        }
        SetBeamToShoot();
    }
    public void SetSelectedItems(int itemID)
    {
        var item = playerItems[itemID];
        bool selected=item.selected=!item.selected;
        buttonEssentials.SetButton(itemID, selected);
    }
    /// <summary>
    /// Disable other beams when the combinations are invalid.
    /// </summary>
    /// <param name="itemID"></param>
    public void DisableIncompatibleBeams(int itemID)
    {
        var items = playerItems;
        if (itemID == 10)
        {
            if (items.ContainsKey(1))
            {
                items[1].selected = false;
                buttonEssentials.SetButton(1, false);
            }
            if (items.ContainsKey(2))
            {
                items[2].selected = false;
                buttonEssentials.SetButton(2, false);
            }
        }else if(itemID==2 || itemID == 1)
        {
            if (items.ContainsKey(10))
            {
                items[10].selected = false;
                buttonEssentials.SetButton(10, false);
            }
        }
    }
    /// <summary>
    /// Checks if item is selected, throught the dictionary of items.
    /// 0=charge beam, 1=ice beam, 2=spazer beam, 3=gravity suit, 4=morfball,
    /// 5=screw attack, 6=bomb,7=high jump, 8=speed booster, 9=gravity jump </summary>
    /// <param name="itemID">item iD to search</param>
    /// <returns>true: if item exist and his selected, false: item don't exist or his not selected yet</returns>
    public bool CheckItem(int itemID)
    {
        if (playerItems.ContainsKey(itemID))
        {
            Item item = playerItems[itemID];
            if (item.selected)return true;
            else return false;
        }else return false;
    }
    public void LoadInventory(GameData data)//List<int> MSB, List<int> item, Dictionary<int,bool> selectItems,Dictionary<int,int> ammoMunition)
    {
        this.data=data;
        int ammo=0;
        var ammoMn=data.ammoMunition;
        for(int i = 1; i <ammoMn.Count; i++)//load limited ammo, from super missiles...
        {
            if (data.ammoMunition.ContainsKey(i))ammo =ammoMn[i];
            CountableAmmo lAmmo=new CountableAmmo(false, i, beams.limitedAmmo[i], ammo);
            limitedAmmo.Add(lAmmo);
            limitedAmmoSearch.Add(lAmmo.iD,lAmmo);
        }
        limitedAmmo[0].maxAmmo = limitedAmmo[0].actualAmmo=ammoMn[0];
        reserve = new List<int>(data.reserve);
        items = new List<int>(data.items);
        foreach(var element in data.selectItems){
            if(element.Key!=4){
                bool value=element.Value;
                AddToItems(new Item(value,element.Key));
                buttonEssentials.SetButton(element.Key,value);
                DisableIncompatibleBeams(element.Key);
            }
        }
        interactions.SetButtonNavigation();
    }
    public void SetSuit()
    {
        if(CheckItem(3))playerSkin.SetGravitySuit();
        else playerSkin.SetPowerSuit();
    }
    /// <summary>
    /// Dynamically select the correct ammo UI.
    /// </summary>
    /// <param name="itemIndex">the ammo index to show</param>
    public int AmmoSelection(int itemIndex)
    {
        for (int i = itemIndex - 1; i < limitedAmmo.Count; i++)
        {
            if(i-1>=0)limitedAmmo[i - 1].Select(false);//previous ammo selected
            if (limitedAmmo[i].CheckAmmo())
            {
                if (i > 0) limitedAmmo[i - 1].Select(false);//previous ammo selected
                limitedAmmo[i].Select(true);
                if (i!=2) canShootBeams = false;
                PlayerInstantiates.countableID = i;
                return itemIndex;
            }
            else
            {
                itemIndex++;//next position in the list
            }
        }
        canShootBeams = true;AmmoSelection();
        return 0;
    }
    #region Mobile Methods
    #if UNITY_ANDROID
    public void AmmoSelection_Mobile(int index){
        var lAmmo=limitedAmmoSearch[index];
        lAmmo.Select(!lAmmo.selected);
        PlayerInstantiates.countableID = index;
        canShootBeams=!lAmmo.selected;
        foreach(var item in limitedAmmoSearch){
            if(item.Key!=index){
                item.Value.Select(false);
                playerInstantiates.beamToShoot=item.Value.ammoPrefab;
            }
        }
    } 
    #endif
    #endregion
    /// <summary>
    /// Disable all ammo UI.
    /// </summary>
    public void AmmoSelection()
    {
        foreach(var element in limitedAmmo)if(element!=null)element.Select(false);
    }
    public void SetGravityJump(int index){
        playerController.OnJump -= playerController.OnNormalJump;
        playerController.OnJump+=playerController.OnGravityJump;
        CheckJumps(index);
    }
    public void SetScrewJump(int index){
        playerController.OnJump -= playerController.OnGravityJump;
        playerController.OnJump += playerController.OnNormalJump;
        CheckJumps(index);

    }
    /// <summary>
    /// Change the player jump force, is called in a unity button event.
    /// </summary>
    /// <param name="changeForce">true: to change the jump force to the high jump force, false: change to the default jump force</param>
    public void ChangeJumpForce()
    {
        if(playerItems.ContainsKey(7)){
            if (playerItems[7].selected) playerController.currentJumpForce = baseData.jumpForceUp;
            else playerController.currentJumpForce = baseData.jumpForce;
        }
    }
    public void SetBeamToShoot()
    {
        if (CheckItem(2))//spazer beam
        {
            if (!CheckItem(1)) Ammo.ammoSelected = 7;//spazer
            else Ammo.ammoSelected = 5;//spazer ice
        }
        else
        {
            if (!CheckItem(1))//ice beam
            {
                if (CheckItem(10)) Ammo.ammoSelected = 6;//plasma
                else Ammo.ammoSelected = 3;//normal
            }
            else Ammo.ammoSelected = 4;//ice
        }
        var gObj=beams.GetAmmoPrefab(Ammo.ammoSelected);
        pool.SetBeamToPool(gObj);
        playerInstantiates.beamToShoot = gObj;
    }
    #endregion
    private void CheckJumps(int id){
        if ((id == 9 && playerItems.ContainsKey(5)) || (id == 5 && playerItems.ContainsKey(9)))
        {
            playerController.OnJump -= playerController.OnNormalJump;
            playerController.OnJump -= playerController.OnGravityJump;
            playerController.OnJump += playerController.OnNormalJump;
            playerController.OnJump += playerController.OnGravityJump;
        }
    }
    private void OnRetry(){
        if(!SaveAndLoad.newGame){
            LoadInventory(data);
            AmmoSelection();
            ChangeJumpForce();
            SetSuit();
            SetBeamToShoot();
        }
    }
}