using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public static class GameEvents
{
    public static Action<bool> playerFire,retry;
    public static Action<float> overHeatAction;
    /// <summary>
    /// Update the player health, when receiving damage and getting health.
    /// </summary>
    public static Action<int,int> playerHealth;
    public static Action healthTank;
    /// <summary>
    /// Refull all player health and inventory ammo.
    /// </summary>
    public static Action refullAll;
    /// <summary>
    /// Event at damage the player. Its neccesary the damage value (int) and
    /// the world position to set a knock back 
    /// </summary>
    public static Action<int, float> damagePlayer;
    public static Action<Vector2> drop;
    public static Action<SaveStation> save;
    public static Action saveMessage;
    public static Action<float,float,Vector3Int,Tile> miniMap;
    public static Action<ICollecteable> verifyRegistry;
}
