using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public interface IPooleable{
    Transform parent{get;set;}
}
public interface IDamageable<T>
{
    void AddDamage(T damage);
}
public interface IInvulnerable{
    bool InvMissiles { get; }
    bool InvSuperMissiles { get; }
    bool InvBeams { get; }
    bool InvBombs { get; }
    bool InvSuperBombs { get; }
    bool InvFreeze { get; }
}
public interface IBeamsInvulnerable{

    bool InvMissiles { get; }
    bool InvSuperMissiles { get; }
    bool InvBeams { get; }
}
public interface IPlayerWeapon{
}
public interface IDrop{

}
public interface IDoDamage
{
    void DoDamage<T,U>(T damage,U enemyH)where U:Health<T>;
}
public interface IFreezeable
{
    void FreezeMe();
    void Unfreeze();
    IEnumerator FreezeVisualFeedBack();
}
public interface IRejectable{
    void Reject();
}
/// <summary>
/// Used like a tag
/// </summary>
public interface ICollecteable{
    
}