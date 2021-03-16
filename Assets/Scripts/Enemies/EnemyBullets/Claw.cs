using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
public class Claw : Weapon,IDrop
{
    new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.GetComponent<IPlayerWeapon>() != null)
        {
            base.DoDrop();
        }
    }
}
