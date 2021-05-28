using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
public class IceRay : Weapon
{
    new void Awake() {
        base.Awake();
    }
    new void FixedUpdate() {
        base.FixedUpdate();
    }
    new private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            IFreezeable ifreezeable=other.GetComponentInParent<IFreezeable>();
            ifreezeable.FreezeMe();
        }
    }
    new private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
