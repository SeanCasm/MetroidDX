using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player.Weapon
{
    public class Plasma : Projectil
    {
        #region Unity methods
        new private void Awake()
        {
            base.Awake();
        }
        new void OnEnable() {
            base.OnEnable();
        }
        new void FixedUpdate()
        {
            base.FixedUpdate();
        }
        new void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
        }
        new void OnBecameInvisible()
        {
            base.OnBecameInvisible();
        }
        #endregion 
    }
}
