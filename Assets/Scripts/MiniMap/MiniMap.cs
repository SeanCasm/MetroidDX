using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MiniMap : MonoBehaviour
{
    public bool itemArea, bossArea;
    public Tile mapTile,unexploredTile;
    public Vector3Int cellPos{get;set;}
    public bool isExplored{get;set;}=false;
    public Tile currentTile{get;private set;}
    public Vector2 newTrans{get;private set;}
    public char spriteSheet{get;private set;}
    private void Start() {
        spriteSheet =mapTile.name[0];
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            isExplored = true;
            currentTile=mapTile;
            cellPos = References.myMap.WorldToCell(transform.position);
            var iconLocation = References.myMap.CellToWorld(cellPos);
            References.myPlayerIcon.position = new Vector3(iconLocation.x+1.8f, iconLocation.y+2f, 0f);
            GameEvents.miniMap.Invoke(this);
        }
    }
    public void SetTile(){
        currentTile=unexploredTile;
        newTrans=new Vector2(transform.position.x+8.195801f,transform.position.y+-115.62f);
        cellPos = References.myMap.WorldToCell(newTrans);
        GameEvents.UnexploredMap.Invoke(this);
    } 
}