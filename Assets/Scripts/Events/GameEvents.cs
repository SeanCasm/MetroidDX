using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public static class GameEvents
{
    /// <summary>
    /// When player fires.
    /// </summary>
    public static Action<bool> playerFire;
    /// <summary>
    /// When player deads.
    /// </summary>
    public static Action<bool> retry;
    /// <summary>
    /// When player selects "retry" in the retry screen. 
    /// </summary>
    public static Action OnRetry;
    /// <summary>
    /// Updates the over heat bar when player fires.
    /// </summary>
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
    /// the world position to set a knock back. 
    /// </summary>
    public static Action<int, float> damagePlayer;
    public static Action<Vector2> drop;
    public static Action<SaveStation> save;
    /// <summary>
    /// Displays the save message when is using the save station.
    /// </summary>
    public static Action saveMessage;
    public static Action<float,float,Vector3Int,Tile> miniMap;
    /// <summary>
    /// Verifies the registry of reserve and items in the collector manager.
    /// </summary>
    public static Action<ICollecteable> verifyRegistry;
    /// <summary>
    /// Enables player after retry game and when is loading for the first time in the main menu.
    /// </summary>
    public static Action enablePlayer;
    /// <summary>
    /// Enables or disables the item buttons in the player item menu.
    /// </summary>
    public static Action<int, bool> setItemButton;
    /// <summary>
    /// Enable the transition when player enters to a door.
    /// </summary>
    public static Action<bool> doorTransition;
    public static Action<bool> timeCounter;
    public static Action<bool> pauseTimeCounter;
    /// <summary>
    /// Sets the background the parallax effect.
    /// </summary>
    public static Action<Transform> parallax;

}