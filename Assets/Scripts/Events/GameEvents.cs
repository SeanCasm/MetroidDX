using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public static class GameEvents
{
    /// <summary>
    /// Updates the player health, when receiving damage and getting health.
    /// </summary>
    public static Action<int,int> playerHealth;
    public static Action healthTank;
    /// <summary>
    /// Refull all player health and inventory ammo.
    /// </summary>
    public static Action refullAll;
    /// <summary>
    /// Event when the player is injured. Its neccesary the damage value (int) and
    /// the world position to set a knock back. 
    /// </summary>
    public static Action<int, float> damagePlayer;
    public static Action<Vector2> drop;
    public static Action<SaveStation> save;
    /// <summary>
    /// Displays the save message when player is using the save station.
    /// </summary>
    public static Action saveMessage;
    public static Action<MiniMap> miniMap;
    public static Action<MiniMap> UnexploredMap;
    /// <summary>
    /// Verifies the registry of reserve and items in the collector manager.
    /// </summary>
    public static Func<int,bool,bool> verifyRegistry;
    /// <summary>
    /// Enables player after retry game and when is loading for the first time in the main menu.
    /// </summary>
    public static Action enablePlayer;
    /// <summary>
    /// Enables the transition when player enters to a door.
    /// </summary>
    public static Action<CameraTransition> DoorTransition;
    public static Action<bool> timeCounter;
    public static Action<bool> pauseTimeCounter;
    /// <summary>
    /// Sets to the background the parallax effect.
    /// </summary>
    public static Action<Transform> parallax;
    /// <summary>
    /// Goes directly to the minimap, used in map updater.
    /// </summary>
    public static Action MinimapShortcout;
    /// <summary>
    /// Starts the transition animation.
    /// </summary>
    public static Func<float> StartTransition;
    public static Action EquipPower,EquipGravity,EquipCorrupt;
    public static Action<string, Input> OnInputBinded;
}