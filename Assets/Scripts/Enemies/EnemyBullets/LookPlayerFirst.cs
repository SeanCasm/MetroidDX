using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy.Weapons{
    public class LookPlayerFirst : Weapon
    {
        [Tooltip("Represents the limit (negative and positive) of the angle when this object looks right toward the player")]
        [SerializeField, Range(1f, 45f)] float rightAngleLimit;
        [Tooltip("Represents the limit (negative and positive) of the angle when this object looks left toward the player")]
        [SerializeField,Range(1f,135f)] float leftAngleLimit;
        new void Start()
        {
            base.Start();
        }
        new void Awake() {
            //base.OnEnable();
            player = References.Player.transform;
        }
        new void OnEnable() {
            base.OnEnable();
            base.SetDirectionAndRotationLimit(rightAngleLimit,leftAngleLimit);
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
 
