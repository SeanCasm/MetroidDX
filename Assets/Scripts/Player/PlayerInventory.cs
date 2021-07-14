using System.Collections.Generic;
using UnityEngine;
using Player;
using Player.Weapon;
[System.Serializable]
public class Inventory
{
    public bool selected { get; set; }
    public int iD { get; set; }
    public Inventory(bool selected, int iD)
    {
        this.selected = selected;
        this.iD = iD;
    }
}
[System.Serializable]
public class Ammo : Inventory
{
    public GameObject ammoPrefab { get; set; }
    public static int ammoSelected;
    public Ammo(bool selected, int iD, GameObject ammoPrefab) : base(selected, iD)
    {
        this.ammoPrefab = ammoPrefab;
    }
}
[System.Serializable]
public class CountableAmmo : Ammo
{
    #region Properties
    public int maxAmmo { get; set; }
    public int actualAmmo { get; set; }

    #endregion
    public CountableAmmo(bool selected, int iD, GameObject ammoPrefab,
        int maxAmmo, int actualAmmo) : base(selected, iD, ammoPrefab)
    {
        this.selected = selected;
        this.iD = iD; this.ammoPrefab = ammoPrefab;
        this.maxAmmo = maxAmmo; this.actualAmmo = actualAmmo;
        GameUI.enableUI.Invoke(iD);
        GameUI.ammoText.Invoke(iD, actualAmmo);
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
        GameUI.enableUI.Invoke(iD);
        GameUI.ammoText.Invoke(iD, actualAmmo);
        GameEvents.refullAll += HandleRefullAll;
    }
    #region Public methods
    public void AddCapacity(int amount)
    {
        maxAmmo += amount; actualAmmo += amount;
        GameUI.ammoText.Invoke(iD, maxAmmo);
    }
    public void ActualAmmoCount(int amount)
    {
        actualAmmo += amount;
        if (actualAmmo >= maxAmmo) actualAmmo = maxAmmo;
        else if (actualAmmo <= 0)
        {
            actualAmmo = 0; Select(false);
        }
        GameUI.ammoText.Invoke(iD, actualAmmo);
    }
    public void Select(bool select)
    {
        GameUI.toggleUI.Invoke(iD, select);
        this.selected = select;
        ammoSelected = iD;
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
    public bool CheckCapacity()
    {
        if (actualAmmo < maxAmmo) return true;
        else return false;
    }
    private void HandleRefullAll()
    {
        this.actualAmmo = maxAmmo;
        GameUI.ammoText.Invoke(iD, maxAmmo);
    }
    public void Unsubcribe()
    {
        GameEvents.refullAll -= HandleRefullAll;
    }
    #endregion
}
/// <summary>
/// Represents the player inventory.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public static int[] beamsID { get; } = { 1, 2, 10 };

    #region Properties
    [SerializeField]Interactions interactions;
    [SerializeField] Beams beams;
    [SerializeField] BaseData baseData;
    [SerializeField] ButtonUtilities buttonEssentials;
    [SerializeField]Pool pool;
    [SerializeField] Gun playerInstantiates;
    public static System.Action GravitySetted;
    public static System.Action GravityUnsetted;
    private GameData data;
    private PlayerController pCont;
    public Dictionary<int, bool> playerItems { get; set; }= new Dictionary<int, bool>();
    public List<int> reserve { get; set; }=new List<int>();
    public bool canShootBeams { get ;set ; }=true;
    //0: missiles, 1: super missiles, 2: super bombs
    public CountableAmmo[] limitedAmmo { get; set; }= new CountableAmmo[4];
    #endregion
    #region Unity methods
     
    void Start()
    {
        pCont = GetComponent<PlayerController>();
        baseData.SetInventoryData(this);
        buttonEssentials.SetButton(4, true);
        interactions.SetButtonNavigation();
        pCont.OnJump+=pCont.SpeedBoosterChecker;
        if (CheckItem(8)) { pCont.OnSpeedBooster += pCont.SpeedBoosterChecker; pCont.MaxSpeed = pCont.SpeedBS; }
        else { pCont.OnSpeedBooster -= pCont.SpeedBoosterChecker; pCont.MaxSpeed = pCont.RunningSpeed; }
        SetBeam();
    }
    private void OnEnable() {
        Retry.Selected -= OnRetry;
        Retry.Selected+=OnRetry;
    }
    void OnDisable()
    {
        foreach(var element in limitedAmmo){if(element!=null)element.Unsubcribe();}
        DisableSelection();
    }
    #endregion
    #region Public methods
    public void AddToItems(int id, bool selected)
    {
        playerItems.Add(id, selected);
        if(id==9 || id==5)SetJumpType(id);
        switch(id){
            case 7: ChangeJumpForce();break;
            case 8: SetSpeedBooster(); break;
            case 3: SetSuit(); break;
        }
        foreach (int element in beamsID)
        {
            if (id == element)DisableIncompatibleBeams(id);
        }
        SetBeam();
    }
    public void SetSelectedItems(int itemID)
    {
        bool item = playerItems[itemID];
        buttonEssentials.SetButton(itemID, item = !item);
        playerItems[itemID] = item;
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
                items[1] = false;
                buttonEssentials.SetButton(1, false);
            }
            if (items.ContainsKey(2))
            {
                items[2] = false;
                buttonEssentials.SetButton(2, false);
            }
        }else if(itemID==2 || itemID == 1)
        {
            if (items.ContainsKey(10))
            {
                items[10] = false;
                buttonEssentials.SetButton(10, false);
            }
        }
    }
    /// <summary>
    /// Checks if a item is selected, throught the dictionary of items.
    /// 0=charge beam, 1=ice beam, 2=spazer beam, 3=gravity suit, 4=morfball,
    /// 5=screw attack, 6=bomb,7=high jump, 8=speed booster, 9=gravity jump, 10=plasma </summary>
    /// <param name="itemID">item iD to search</param>
    /// <returns>true: if item exist and is selected, false: item doesn't exist or is not selected yet</returns>
    public bool CheckItem(int itemID)
    {
        if (playerItems.ContainsKey(itemID))
        {
            bool selected = playerItems[itemID];
            if (selected)return true;
            else return false;
        }else return false;
    }
    public bool CheckLimitedAmmo(int id){
        for(int i=0;i<limitedAmmo.Length;i++){
            if(limitedAmmo[i]!=null){
                if(limitedAmmo[i].iD == id)return true;
            }
        }
        return false;
    }
    public void LoadInventory(GameData data)
    {
        this.data=data;
        int ammo=0;
        var ammoMn=data.ammoMunition;
        for(int i = 1; i <ammoMn.Count; i++)//load limited ammo, from super missiles...
        {
            if (data.ammoMunition.ContainsKey(i))ammo =ammoMn[i];
            CountableAmmo lAmmo=new CountableAmmo(false, i, beams.limitedAmmo[i], ammo);
            limitedAmmo[i]=lAmmo;
        }
        limitedAmmo[0].maxAmmo = limitedAmmo[0].actualAmmo=ammoMn[0];
        reserve = new List<int>(data.reserve);
        foreach(var element in data.items){
            if(element.id!=4){
                bool value=element.selected;
                AddToItems(element.id,value);
                print(element);
                buttonEssentials.SetButton(element.id,value);
                DisableIncompatibleBeams(element.id);
            }
        }
        interactions.SetButtonNavigation();
    }
    public void SetSuit()
    {
        if(CheckItem(3))GameEvents.EquipGravity.Invoke();
        else GameEvents.EquipPower.Invoke();
    }
    /// <summary>
    /// Dynamically select the correct ammo UI.
    /// </summary>
    /// <param name="itemIndex">the ammo index to show</param>
    public int AmmoSelection(int itemIndex)
    {
        for (int i = itemIndex; i < limitedAmmo.Length; i++)
        {
            if (itemIndex - 1 >= 0 && limitedAmmo[itemIndex - 1] != null) limitedAmmo[itemIndex - 1].Select(false);//previous ammo selected
            if (limitedAmmo[i]!=null && limitedAmmo[i].CheckAmmo())
            {
                limitedAmmo[i].Select(true);
                if (i!=2){
                    canShootBeams = false;
                    pool.SetBeamToPool(limitedAmmo[i].ammoPrefab);
                }
                Gun.countableID = i;
                return itemIndex;
            }
            else
            {
                itemIndex++;//next position in the list
            }
        }
        DisableSelection();
        return -1;
    }
    #region Mobile Methods
#if UNITY_ANDROID
    public void AmmoSelection_Mobile(int index){
        var lAmmo=limitedAmmoSearch[index];
        lAmmo.Select(!lAmmo.selected);
        canShootBeams = lAmmo.selected;
        if(lAmmo.selected){
            PlayerInstantiates.countableID = index;
            pool.SetBeamToPool(lAmmo.ammoPrefab);
        }else AmmoSelection();
    } 
#endif
    #endregion
    /// <summary>
    /// Disables all ammo UI.
    /// </summary>
    public void DisableSelection()
    {
        foreach(var element in limitedAmmo)if(element!=null)element.Select(false);
        SetBeam();
    }
    public void SetJumpType(int id)
    {
        if(CheckItem(id)){
            if(id==9){
                pCont.gravityJump=true;
                pCont.OnJump -= pCont.OnNormalJump;
                pCont.OnJump+=pCont.OnGravityJump;
                GravitySetted?.Invoke();
            }else pCont.screwSelected=true;
            
        }else{
            if (id == 9){
                pCont.gravityJump = false;
                pCont.OnJump -= pCont.OnGravityJump;
                pCont.OnJump+=pCont.OnNormalJump;
                GravityUnsetted?.Invoke();
            }else{
                if(pCont.OnRoll)PlayerHealth.invulnerability=false;
                pCont.screwSelected = false;
            }
        }
    }
    /// <summary>
    /// Changes the player jump force, called in a unity button event.
    /// </summary>
    /// <param name="changeForce">true: change the jump force to the high jump force, false: change to the default jump force</param>
    public void ChangeJumpForce()
    {
        if(playerItems.ContainsKey(7)){
            pCont.currentJumpForce=playerItems[7]?baseData.jumpForceUp:baseData.jumpForce;
        }
    }
    public void SetSpeedBooster(){
        if(!CheckItem(8)){pCont.OnSpeedBooster+=pCont.SpeedBoosterChecker;pCont.MaxSpeed=pCont.SpeedBS;}
        else{ pCont.OnSpeedBooster -= pCont.SpeedBoosterChecker;pCont.MaxSpeed = pCont.RunningSpeed; }
    }
    public void SetBeam()
    {
        canShootBeams = true;
        if (CheckItem(2))//spazer beam
        {
            Ammo.ammoSelected=CheckItem(1)?5:7;
        }
        else
        {
            if (!CheckItem(1))//ice beam
            {
                Ammo.ammoSelected=CheckItem(10)?6:3;
            }
            else Ammo.ammoSelected = 4;//ice
        }
        if(CheckItem(0))pool.SetChargedBeamToPool(beams.GetAmmoPrefab(-Ammo.ammoSelected));
        var gObj=beams.GetAmmoPrefab(Ammo.ammoSelected);
        pool.SetBeamToPool(gObj);
        playerInstantiates.beamToShoot = gObj;
    }
    #endregion
     
    private void OnRetry(){
        if(!SaveAndLoad.newGame){
            LoadInventory(data);
            DisableSelection();
            ChangeJumpForce();
            SetSuit();
            SetBeam();
        }
    }
}