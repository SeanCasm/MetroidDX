using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy.Weapons{
    public class LookPlayerFirst : Weapon
    {
        new void Awake() {
            base.Awake();
            player = References.Player.transform;
        }
        protected new void OnEnable() {
            base.OnEnable();
            base.SetDirectionAndRotation();
        }
        new void OnTriggerEnter2D(Collider2D other) {
            base.OnTriggerEnter2D(other);
        }
        new void OnBecameInvisible() {
            base.OnBecameInvisible();
        }
        new void FixedUpdate() {
           base.FixedUpdate(); 
        }
    }
}
 
