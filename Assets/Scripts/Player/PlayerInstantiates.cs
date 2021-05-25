using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
using System;

public class PlayerInstantiates : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] Beams beams;
    private GameObject chargingLoad;
    private PlayerInventory inventory;
    public GameObject beamToShoot{get;set;}
    public static int countableID;
    void Awake()
    {
        chargingLoad = transform.GetChild(0).gameObject;
        inventory = GetComponentInParent<PlayerInventory>();
    }
    private void OnEnable() {
        GameEvents.playerFire += GunShoot;
    }
    private void OnDisable() {
        GameEvents.playerFire -= GunShoot;
    }
    #region Private methods
    private void ShootPrefab(GameObject prefab)
    {
        GameObject mb = Instantiate(prefab, firePoint.position, firePoint.rotation,null) as GameObject;
        Beam bulletComponent = mb.GetComponent<Beam>();
        bulletComponent.Direction=transform.right;
    }
    #endregion
    #region Public methods
    public void Charge(bool value)
    {
        chargingLoad.SetActive(value);
    }
    private void GunShoot(bool isCharging)
    {
        var ammo = inventory.limitedAmmoSearch;
        if(countableID==2)countableID=-999;
        if(inventory.canShootBeams){
            if(!isCharging) ShootPrefab(beamToShoot);
            else{
                int id=beamToShoot.GetComponent<Beam>().ID;
                beamToShoot = beams.GetAmmoPrefab(id * -1);
                ShootPrefab(beamToShoot);
                //back to normal beam
                beamToShoot=beams.GetAmmoPrefab(id);
            }
        }else{
            if (ammo.ContainsKey(countableID))
            {
                var ammoPos = ammo[countableID];
                ShootPrefab(ammoPos.ammoPrefab);
                ammoPos.ActualAmmoCount(-1);
                if (ammoPos.actualAmmo <= 0) inventory.canShootBeams = true;
            }
        }
    }
    PlayerInventory.CountableAmmo ammo;
    private bool canInstantiate = true; 
    public void SetBomb()
    {
        bool ins=false;;
        if (inventory.limitedAmmoSearch.ContainsKey(2))
        {
            
            ammo = inventory.limitedAmmoSearch[2];
            if(ammo.selected)ins=true;
        }
        else
        {
            ammo = null;
        }
        if(ins){
            if (ammo != null && ammo.CheckAmmo())
            {
                GameObject mb = Instantiate(ammo.ammoPrefab, firePoint.position, Quaternion.identity) as GameObject;
                ammo.ActualAmmoCount(-1);
            }
            else if (inventory.CheckItem(6) && canInstantiate)//bomb
            {
                GameObject mb = Instantiate(beams.bomb, firePoint.transform.position, Quaternion.identity);
                canInstantiate = false;
                Invoke("InsBomb", 1f);
            }
        }
    }
    void InsBomb()
    {
        canInstantiate = true;
    }
    #endregion
}