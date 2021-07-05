using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
public class Throw : Weapon
{
    [SerializeField] float timeTillHit;
    new void Awake() {
       base.OnEnable(); 
    }
    new void OnEnable()
    {
        Invoke("BackToShootPoint", livingTime);
    }
    new void OnBecameInvisible() {
        base.OnBecameInvisible();
    }
    new void FixedUpdate() {
        
    }
    // <summary>
    /// Make a parabolic trajectory through the bullet throw point and target
    /// </summary>
    /// <param name="throwPoint">transform of throw point</param>
    /// <param name="target">transform of the target point</param>
    public void ThrowPrefab(Transform throwPoint, Transform target)
    {
        float xdistance;
        xdistance = target.position.x - throwPoint.position.x;
        float ydistance;
        ydistance = target.position.y - throwPoint.position.y;
        float throwAngle;
        throwAngle = Mathf.Atan((ydistance + 4.905f * (timeTillHit * timeTillHit)) / xdistance);
        float totalVelo = xdistance / (Mathf.Cos(throwAngle) * timeTillHit);
        float xVelo, yVelo;
        xVelo = totalVelo * Mathf.Cos(throwAngle);
        yVelo = totalVelo * Mathf.Sin(throwAngle);
        rigid.velocity = new Vector2(xVelo, yVelo);
    }
}
