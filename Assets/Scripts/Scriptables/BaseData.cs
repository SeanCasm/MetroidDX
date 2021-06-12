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
        [Header("Suit")]
        [Tooltip("Sprite used in the item menu.")]
        public Sprite powerSuit;
        [Tooltip("Sprite used in the item menu.")]
        public Sprite gravitySuit;
        public Sprite[] gravityCompleteSheet;
        [SerializeField] Beams beams;
        public void SetInventoryData(PlayerInventory inventory){
            inventory.limitedAmmo.Add(new PlayerInventory.CountableAmmo(false, 0, beams.limitedAmmo[0], missileAmmo, missileAmmo));
            inventory.limitedAmmoSearch.Add(0, inventory.limitedAmmo[0]);
            inventory.AddToItems(new PlayerInventory.Item(true, 4));//add the morfball item, initial game item by default.
            inventory.AddToItems(new PlayerInventory.Item(true, 8));//add the speed booster item, for debug purposes.
            inventory.transform.position=spawn;
        }
        public void SetHealthData(PlayerHealth playerHealth){
            playerHealth.MaxTotalHealth=playerHealth.MyHealth=health;
            playerHealth.Tanks=energyTanks;
            playerHealth.TotalHealth = totalHealth;
        }
        /// <summary>
        /// Set the player suit, passing the suit name without spaces
        /// </summary>
        /// <param name="name">Suit name</param>
        public Sprite SetSuit(string name){
            switch(name.ToLower()){
                case "powersuit":
                return powerSuit;
                case "gravitysuit":
                return gravitySuit;
            }
            return null;
        }
    }
}
 
