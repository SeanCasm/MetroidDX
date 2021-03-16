using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MiniMap : MonoBehaviour
{
    public bool itemArea, bossArea;
    public Tile mapTile;
    private Vector3Int cellPos;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            cellPos = References.myMap.WorldToCell(transform.position);
            var iconLocation = References.myMap.CellToWorld(cellPos);
            References.myPlayerIcon.position = new Vector3(iconLocation.x+1.8f, iconLocation.y+2f, 0f);
            GameEvents.miniMap.Invoke(transform.GetX(),transform.GetY(),cellPos,mapTile);
        }
    }
     
}