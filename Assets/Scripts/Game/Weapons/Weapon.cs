using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
namespace Player.Weapon
{
    public class Weapon : WeaponBase<float>,IPlayerWeapon
    {
        [SerializeField] protected float hotPoints;
        [SerializeField] protected int iD;
        [SerializeField] protected WeaponType beamType;
        public WeaponType BeamType=>beamType;
        protected Rigidbody2D rigid;
        protected bool rejected;
        public int ID{get=>iD;}

        public void TryDoDamage(float damage, IDamageable<float> healthManager, WeaponType beamType,IInvulnerable iInvulnerable)
        {
            switch (beamType)
            {
                case WeaponType.Beam:
                    if (!iInvulnerable.InvBeams) healthManager.AddDamage(damage);
                    else rejected=true;
                    break;
                case WeaponType.Missile:
                    if (!iInvulnerable.InvMissiles) healthManager.AddDamage(damage);
                    else rejected=true;
                    break;
                case WeaponType.SuperMissile:
                    if (!iInvulnerable.InvSuperMissiles) healthManager.AddDamage(damage);
                    else rejected = true;
                    break;
                case WeaponType.Bomb:
                    if (!iInvulnerable.InvBombs) healthManager.AddDamage(damage);
                    break;
                case WeaponType.SuperBomb:
                    if (!iInvulnerable.InvSuperBombs) healthManager.AddDamage(damage);
                    break;
            }
        }
    }
}
