using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
namespace Player.Weapon
{
    public class Missile : Projectil, IRejectable
    {
         
        private BoxCollider2D damageCol;


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
        new void OnDisable() {
            base.OnDisable();
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
        new void Reject()
        {
            base.Reject();
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.constraints = RigidbodyConstraints2D.None;
            speed = 0;
            rigid.gravityScale = 7f;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
