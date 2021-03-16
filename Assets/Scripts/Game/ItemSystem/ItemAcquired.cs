using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Items;
public class ItemAcquired : MonoBehaviour,ICollecteable
{
    #region Properties
    [SerializeField] string message;
    [SerializeField] int iD;
    [SerializeField] ItemType itemType;
    private BoxCollider2D box;
    private Animator anim;
    public event Action<ItemAcquired> onPickup;

    public int ID { get { return iD; } }
    public string Message { get { return message; } }
    public ItemType iType { get { return itemType; } }
    #endregion
    #region Unity Methods
    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        GameEvents.verifyRegistry.Invoke(this);
    }
    private void OnBecameInvisible()
    {
        anim.enabled =box.enabled = false;
    }
    private void OnBecameVisible()
    {
        anim.enabled=box.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onPickup.Invoke(this);
        }
    }
    private void OnDisable()
    {
        var acqSystem = References.AcqSystem;
        onPickup -= acqSystem.HandlePickupItem;
        acqSystem.itemSearch[ID] = null;
    }
    #endregion
}
