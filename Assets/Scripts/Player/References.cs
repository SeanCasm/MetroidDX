using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
public class References : MonoBehaviour
{
    [SerializeField] Tilemap map;
    [SerializeField] Transform playerIcon;
    public static Transform myPlayerIcon { get; set; }
    public static Tilemap myMap { get; set; }
    void OnEnable()
    {
        myMap = map;
        myPlayerIcon = playerIcon;
    }
}
