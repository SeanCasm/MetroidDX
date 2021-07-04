using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
using System;

public class PlayerInstantiates : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] Beams beams;
    [SerializeField] Pool pool;
    private GameObject chargingLoad;
    private PlayerInventory inventory;
    public GameObject beamToShoot{get;set;}
    public static int countableID;
    private int shoots;
    void Awake()
    {
        chargingLoad = transform.GetChild(0).gameObject;
        inventory = GetComponentInParent<PlayerInventory>();
    }
    private void OnEnable() {
        shoots=0;
        GameEvents.playerFire += GunShoot;
    }
    private void OnDisable() {
        GameEvents.playerFire -= GunShoot;
    } 
    #region Public methods
    public void Charge(bool value)
    {
        chargingLoad.SetActive(value);
    }
    private void GunShoot(bool isCharging)
    {
        var ammo = inventory.limitedAmmo;
        if(countableID==2)countableID=-999;
        if(inventory.canShootBeams){
            if(!isCharging) pool.ActiveNextPoolObject();
            else{
                int id=beamToShoot.GetComponent<Beam>().ID;
                beamToShoot = beams.GetAmmoPrefab(id * -1);
                pool.ActiveNextChargedPoolObject();
                //back to normal beam
                beamToShoot=beams.GetAmmoPrefab(id);
            }
        }else{
            if (inventory.CheckLimitedAmmo(countableID))
            {
                var ammoPos = ammo[countableID];
                pool.ActiveNextPoolObject();
                ammoPos.ActualAmmoCount(-1);
                if (ammoPos.actualAmmo <= 0) inventory.canShootBeams = true;
            }
        }
    }
    CountableAmmo ammo;
    private bool canInstantiate = true; 
    public void SetBomb()
    {
        ammo=null;
        if(inventory.CheckLimitedAmmo(2))ammo=inventory.limitedAmmo[2];
        if(ammo!=null){
            if (ammo.CheckAmmo() && ammo.selected)
            {
                GameObject mb = Instantiate(ammo.ammoPrefab, firePoint.position, Quaternion.identity) as GameObject;
                ammo.ActualAmmoCount(-1);
            }
        }
        if((ammo==null || !ammo.CheckAmmo()) && inventory.CheckItem(6) && canInstantiate){
            GameObject mb = Instantiate(beams.bomb, firePoint.transform.position, Quaternion.identity);
            canInstantiate = false;
            Invoke("InsBomb", .5f);
        }
    }
    void InsBomb()
    {
        canInstantiate = true;
    }
    #endregion
}