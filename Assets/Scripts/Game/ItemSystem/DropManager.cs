using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DropManager : MonoBehaviour
{
    [SerializeField] ScriptableDrop drop;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerInventory inventory;
    private void OnEnable() {
        GameEvents.drop+=DropSomething;
    }
    private void OnDisable() {
        GameEvents.drop-=DropSomething;
    }
    private void DropSomething(Vector2 position)
    {
        GameObject newDrop=drop.DropIntermediary(playerHealth, inventory);
        if(newDrop!=null)Instantiate(newDrop,position,Quaternion.identity,null);
    }
}
