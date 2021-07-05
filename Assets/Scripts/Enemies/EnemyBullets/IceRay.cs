using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
public class IceRay : Weapon
{
    new void Awake() {
        base.OnEnable();
        player = References.Player.transform;
    }
    new void FixedUpdate() {
        base.FixedUpdate();
    }
    new void OnEnable()
    {
        base.OnEnable();
        base.SetDirectionAndRotation();
    }
    new private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            IFreezeable ifreezeable=other.GetComponentInParent<IFreezeable>();
            ifreezeable.FreezeMe();
        }
    }
    new private void OnBecameInvisible() {
        base.OnBecameInvisible();
    }
}
