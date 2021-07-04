using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Items;
public class ReserveAcquired : MonoBehaviour,ICollecteable
{
    #region Properties
    [SerializeField] ReserveType reserveType;
    [SerializeField] int iD;
    private BoxCollider2D box;
    private Animator anim;
    public string nameItem { get; set; }
    public int ID { get { return iD; } }
    #endregion
    #region Public Methods
    public string Name { get { return name; } set { name = value; } }
    public ReserveType ItemType { get { return reserveType; } }
    
    #endregion
    #region Unity Methods
    private void Awake()
    {
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        nameItem=gameObject.name;
        GameEvents.verifyRegistry.Invoke(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            CollectorManager.instance.HandlePickupReserve(this);
        }
    }
    private void OnBecameVisible()
    {
        anim.enabled=box.enabled = true;
    }
    private void OnBecameInvisible()
    {
        box.enabled = anim.enabled =false;
    }
  
    #endregion
}