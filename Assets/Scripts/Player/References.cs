using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
public class References : MonoBehaviour
{
    [SerializeField] Tilemap map;
    [SerializeField] Transform playerIcon;
    [SerializeField] GameObject player;
    public static Transform myPlayerIcon { get; set; }
    public static Tilemap myMap { get; set; }
    public static GameObject Player{get;private set;}
    private void Awake() {
        Player = player;
        myMap = map;
        myPlayerIcon = playerIcon;
    }
}
