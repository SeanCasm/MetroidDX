using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class MapSaveSystem : MonoBehaviour
{
    #region Properties
    [SerializeField] Tilemap tileMap;
    public List<(string name, int xpos, int ypos, char spriteSheet)> tileInfo { get; set; } = new List<(string name, int xpos, int ypos, char spriteSheet)>();
    public bool[] miniMapItem { get; set; }
    public static Dictionary<float, float> tilesRegistered { get; set; }//x,y

    public List<int> mappers = new List<int>();
    #endregion
    #region Unity Methods
    void Awake()
    {
        tilesRegistered = new Dictionary<float, float>();
        miniMapItem = new bool[40];
    }
    void OnEnable()
    {
        GameEvents.miniMap += HandleRegistryTile;
        GameEvents.UnexploredMap += xd;
    }
    private void OnDisable()
    {
        GameEvents.miniMap -= HandleRegistryTile;
        GameEvents.UnexploredMap -= xd;
    }
    #endregion
    #region Private Methods
    private void HandleRegistryTile(MiniMap miniMap)
    {
        tileMap.SetTile(miniMap.cellPos, miniMap.currentTile);
        tileInfo.Add((miniMap.mapTile.name, miniMap.cellPos.x, miniMap.cellPos.y, miniMap.spriteSheet));
    }
    void xd(MiniMap miniMap)
    {
        if (tileMap.GetTile(tileMap.WorldToCell(miniMap.newTrans)) == null)
        {
            tileMap.SetTile(miniMap.cellPos, miniMap.currentTile);
            tileInfo.Add((miniMap.name, miniMap.cellPos.x, miniMap.cellPos.y, miniMap.spriteSheet));
        }
    }
    private void SetTilesToTilemap()
    {
        Dictionary<string,Tile> tiles1=LoadFromResources("0");
        Dictionary<string,Tile> tiles2=LoadFromResources("1");
        Dictionary<string,Tile> tiles3=LoadFromResources("2");
        Dictionary<string,Tile> tiles4=LoadFromResources("3");
        Dictionary<string,Tile> tiles5=LoadFromResources("4");
        
        Tile tile = new Tile();
        tileInfo.ForEach(item =>
        {
            Vector3Int newPos = new Vector3Int(item.xpos, item.ypos, 0);
            switch (item.spriteSheet)
            {
                case '0':
                    tile=tiles1[item.name];
                    break;
                case '1':
                    tile = tiles2[item.name];
                    break;
                case '2':
                    tile = tiles3[item.name];
                    break;
                case '3':
                    tile = tiles4[item.name];
                    break;
                case '4':
                    tile = tiles5[item.name];
                    break;
            }
            tileMap.SetTile(newPos,tile);
        });
    }
    private Dictionary<string,Tile> LoadFromResources(string route){
        return Resources.LoadAll(route, typeof(Tile)).Cast<Tile>().ToDictionary(item=>item.name,item=>item);
    }
    #endregion
    /// <summary>
    /// Load data to the MapSaveSystem.
    /// </summary>
    /// <param name="data">the game data</param>
    public void LoadMap(GameData data)
    {
        tileMap.ClearAllTiles();
        tilesRegistered = new Dictionary<float, float>(data.tilesRegistered);
        miniMapItem = data.miniMapItem;
        tileInfo = new List<(string name, int xpos, int ypos,char spriteSheet)>(data.tileInfo);
        SetTilesToTilemap();
    }
}