using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drop", menuName = "ScriptableObjects/Drop")]
public class ScriptableDrop : ScriptableObject
{
    public GameObject[] lifePrefabs;
    public GameObject[] ammoPrefabs;
    public GameObject reloadAll;
    /// <summary>
    /// Checks the list of limited ammo for actualAmmo lower than maxAmmo.
    /// </summary>
    /// <param name="playerI"></param>
    /// <returns></returns>
    private bool CheckForDrop(PlayerInventory playerI)
    {
        for (int i = 0; i < playerI.limitedAmmo.Count; i++)
        {
            return playerI.limitedAmmo[i].CheckCapacity();
        }
        return false;
    }
    private bool CheckForDrop(PlayerHealth playerH){
        if(playerH.MyHealth < 99 && playerH.CurrentMaxTotalHealth<playerH.MaxTotalHealth){
            return true;
        }
        return false;
    }
    private GameObject SendHealthDrop(){
        return lifePrefabs[Random.Range(0,lifePrefabs.Length)];
    }
    private GameObject SendAmmoDrop(PlayerInventory playerInventory)
    {
        var countableAmmo=playerInventory.limitedAmmoSearch;
        List<GameObject> ammoOnInventory=new List<GameObject>();
        for(int i=0;i<3;i++){
            if (countableAmmo.ContainsKey(i)){
                var ammo=countableAmmo[i];
                if(ammo.actualAmmo<ammo.maxAmmo) ammoOnInventory.Add(ammoPrefabs[i]);
            }
        }
        return ammoOnInventory.GetRandom();
    }
    public GameObject DropIntermediary(PlayerHealth playerHealth,PlayerInventory playerInventory){
        bool health=CheckForDrop(playerHealth);
        bool ammo=CheckForDrop(playerInventory);
        if(health && ammo){
            if(Random.Range(0,1)==0)return SendHealthDrop();
            else return SendAmmoDrop(playerInventory);
        }else if(health && !ammo){
            return SendHealthDrop();
        }else if(!health && ammo){
            return SendAmmoDrop(playerInventory);
        }else{
            return null;
        }
    } 
}
