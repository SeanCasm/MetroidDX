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
    public event Action<ReserveAcquired> onPickup;
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
        switch (reserveType)
        {
            case ReserveType.Missile:
                nameItem = "Missile";
                break;
            case ReserveType.SuperBomb:
                nameItem = "Super bomb";
                break;
            case ReserveType.SuperMissile:
                nameItem = "Super missile";
                break;
            case ReserveType.EnergyTank:
                nameItem = "Energy tank";
                break;
        }
        GameEvents.verifyRegistry.Invoke(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))onPickup.Invoke(this);
    }
    private void OnBecameVisible()
    {
        anim.enabled=box.enabled = true;
    }
    private void OnBecameInvisible()
    {
        box.enabled = anim.enabled =false;
    }
    private void OnDisable()
    {
        var acqSystem = References.AcqSystem;
        onPickup -= acqSystem.HandlePickupReserve;
        acqSystem.reserveSearch[ID] = null;
    }
    #endregion
}