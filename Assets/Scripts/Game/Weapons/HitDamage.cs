using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDamage : IDoDamage
{
    public void DoDamage<T, U>(T damage, U enemyH) where U : Health<T>
    {
     //   enemyH.AddDamage(damage);
    }
}
public class IceDamage : IDoDamage
{
    public void DoDamage<T, U>(T damage, U enemyH) where U : Health<T>
    {
       // enemyH.AddDamage(damage);
    }
}
