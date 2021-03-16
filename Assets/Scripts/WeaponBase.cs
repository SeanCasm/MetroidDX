using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponBase<T> : MonoBehaviour
{
    [Header("Weapon settings")]
    [SerializeField] protected T damage;
    [SerializeField] protected float livingTime;
    [SerializeField] protected GameObject startSound;
    //protected IDoDamage damageType;
    protected void Awake() {
        if(startSound!=null)Instantiate(startSound);
    }
}

