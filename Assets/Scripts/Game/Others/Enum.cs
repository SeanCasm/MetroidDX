using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player.Weapon
{
    public enum WeaponType{
        Missile, SuperMissile, Beam, IceBeam,Plasma,Bomb, SuperBomb,All
    }
}
public enum Input
{
    Select, Back
}
public enum CameraTransition{
    Left,Right,Up,Down
}
namespace Enemy.Weapon{
    public enum WeaponEffects{
        None,Freeze
    }
}
namespace Items{
    public enum ReserveType
    {
        Missile, SuperMissile, SuperBomb, EnergyTank,BouncingBomb
    }
    public enum ItemType
    {
        Special, Suit,Beam
    }
    public enum CollectibleType{
        Missile,SuperMissile,SuperBomb,Health,Special
    }
}
namespace Blocks{
    public enum BlockType{
        beam, missile, bomb, superMissile, superBomb, screwAttack, speedBooster
    }
}
namespace Game.Device{
    public enum Device
    {
        Gamepad, Keyboard
    }
}
