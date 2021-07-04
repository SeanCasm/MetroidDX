using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player{
    [CreateAssetMenu(fileName="new BaseData",menuName="ScriptableObjects/Player/BaseData")]
    public class BaseData : ScriptableObject
    {
        [Header("Player inventory")]
        public int missileAmmo;
        [Header("Player health")]
        public int health;
        public int totalHealth;
        public int energyTanks;
        [Header("Misc")]
        public Vector3 spawn;
        public float jumpForce,jumpForceUp;
        [SerializeField] Beams beams;
        public void SetInventoryData(PlayerInventory inventory){
            inventory.limitedAmmo[0]=new CountableAmmo(false, 0, beams.limitedAmmo[0], missileAmmo, missileAmmo);
            inventory.AddToItems(new Item(true, 4));//add the morfball item, initial game item by default.
            inventory.transform.position=spawn;
            inventory.SetSuit();
        }
        public void SetHealthData(PlayerHealth playerHealth){
            playerHealth.MaxTotalHealth=playerHealth.MyHealth=health;
            playerHealth.Tanks=energyTanks;
            playerHealth.TotalHealth = totalHealth;
        }
    }
}
 
