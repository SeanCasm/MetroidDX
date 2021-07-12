using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
public class IceRay : Weapon
{
    [Tooltip("Represents the limit (negative and positive) of the angle when this object looks to the player")]
    [SerializeField, Range(1f, 45f)] float angleLimit;
    [Tooltip("Represents the limit (negative and positive) of the angle when this object looks left toward the player")]
    [SerializeField, Range(1f, 135f)] float leftAngleLimit;
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
        base.SetDirectionAndRotationLimit(angleLimit,leftAngleLimit);
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
