using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShooteableBlock : DestroyableBlock
{
    #region Unity methods
    new void Awake()
    {
        base.Awake();
    }
    new void Start()
    {
        base.Start();
    }
    new void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
    }
    #endregion
}