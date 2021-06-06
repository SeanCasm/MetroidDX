using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
public class MapSaveSystem : MonoBehaviour
{
    #region Properties
    [SerializeField] Tilemap tileMap;
    /// <summary>
    /// Saves the x position of tiles by name. 
    /// </summary>
    /// <value></value>
    public Dictionary<string, List<int>> tilesX { get; set; }
    public Dictionary<string, List<int>> tilesY { get; set; }
    public bool[] miniMapItem { get; set; }
    public static Dictionary<float,float> tilesRegistered{get;set;}//x,y

    public List<int> mappers=new List<int>();
    #endregion
    #region Unity Methods
    void Awake()
    {
        tilesRegistered=new Dictionary<float, float>();
        tilesX = new Dictionary<string, List<int>>();
        tilesY = new Dictionary<string, List<int>>();
        miniMapItem = new bool[40];
    }
    void OnEnable(){
        GameEvents.miniMap+=HandleRegistryTile;
        GameEvents.xd+=xd;
    }
    private void OnDisable() {
        GameEvents.miniMap-=HandleRegistryTile;
        GameEvents.xd -= xd;
    }
    #endregion
    #region Private Methods
    private void HandleRegistryTile(MiniMap miniMap){
            print("XD");
            tileMap.SetTile(miniMap.cellPos, miniMap.currentTile);
            SaveMap(miniMap.currentTile,miniMap.cellPos);
    }
    void xd(MiniMap miniMap){
        if (tileMap.GetTile(tileMap.WorldToCell(miniMap.newTrans)) == null  )
        {
            print("XD");
            tileMap.SetTile(miniMap.cellPos, miniMap.currentTile);
            SaveMap(miniMap.currentTile, miniMap.cellPos);
        }
    }
    private void SaveMap(Tile mapTile,Vector3Int cellPos)
    {
        List<int> localSaveX = new List<int>();
        if (tilesX.ContainsKey(mapTile.name)) localSaveX = tilesX[mapTile.name];

        localSaveX.Add(cellPos.x);

        List<int> localSaveY = new List<int>();
        if (tilesY.ContainsKey(mapTile.name)) localSaveY = tilesY[mapTile.name];

        localSaveY.Add(cellPos.y);

        tilesX[mapTile.name] = localSaveX;
        tilesY[mapTile.name] = localSaveY;
    }
    /// <summary>
    /// Load the tilemap game Map, loading the tiles from the Resources folder and checking if
    /// the name of the saved tiles in dictionarys match with tiles name from Resources folder.
    /// The variable tilesX is used like a reference to get the total tiles registered.
    /// </summary>
    private void SetTilesToTilemap()
    {
        //Load tiles without doors
        var blueNoDoors = Resources.LoadAll("BlueTilesND", typeof(Tile)).Cast<Tile>().ToArray();
        //Load tiles with doors
        var blueDoors= Resources.LoadAll("BlueTilesD",typeof(Tile)).Cast<Tile>().ToArray();
        //Load tiles with save icon
        var saveTiles=Resources.LoadAll("SaveTiles",typeof(Tile)).Cast<Tile>().ToArray();
        //Load unexplored tiles with doors
        var unDoors=Resources.LoadAll("UnTilesD",typeof(Tile)).Cast<Tile>().ToArray();
        //Load unexplored tiles without doors
        var unNoDoors = Resources.LoadAll("UnTilesND", typeof(Tile)).Cast<Tile>().ToArray();
        Dictionary<string, Tile> tilesND = new Dictionary<string, Tile>();
        Dictionary<string, Tile> tilesD = new Dictionary<string, Tile>();
        Dictionary<string, Tile> saves = new Dictionary<string, Tile>();
        Dictionary<string,Tile> tileUND=new Dictionary<string, Tile>();
        Dictionary<string, Tile> tileUD = new Dictionary<string, Tile>();
        
        foreach (Tile element in blueNoDoors)
        {
            tilesND.Add(element.name, element);
        }
        foreach(Tile element in blueDoors){
            tilesD.Add(element.name,element);
        }
        foreach(Tile element in saveTiles){
            saves.Add(element.name,element);
        }
        foreach (Tile element in unDoors)
        {
            tileUD.Add(element.name, element);
        }
        foreach (Tile element in unNoDoors)
        {
            tileUND.Add(element.name, element);
        }
        Vector3Int coordinates = new Vector3Int();
        List<int> tilesX_Temp = new List<int>();
        List<int> tilesY_Temp = new List<int>();
        var tilesX_Keys = tilesX.Keys.ToArray();
        var tilesY_Keys = tilesY.Keys.ToArray();
        Tile mapTile;
        /*for loops trought tilesX because its used for reference to the total tiles are
        registered, considering repeated tiles keys (Tile.name) and they're ocurrences
        */
        for (int i = 0; i < tilesX.Count; i++)
        {
            if (tilesND.ContainsKey(tilesX_Keys[i]))
            {
                mapTile = tilesND[tilesX_Keys[i]];
                tilesX_Temp = tilesX[tilesX_Keys[i]];
                tilesY_Temp = tilesY[tilesY_Keys[i]];
                for (int j = 0; j < tilesX_Temp.Count; j++)
                {
                    coordinates.x = tilesX_Temp[j];
                    coordinates.y = tilesY_Temp[j];
                    tileMap.SetTile(coordinates, mapTile);
                }
            }else if(tilesD.ContainsKey(tilesX_Keys[i])){
                mapTile = tilesD[tilesX_Keys[i]];
                tilesX_Temp = tilesX[tilesX_Keys[i]];
                tilesY_Temp = tilesY[tilesY_Keys[i]];
                for (int j = 0; j < tilesX_Temp.Count; j++)
                {
                    coordinates.x = tilesX_Temp[j];
                    coordinates.y = tilesY_Temp[j];
                    tileMap.SetTile(coordinates, mapTile);
                }
            }else if(saves.ContainsKey(tilesX_Keys[i])){
                mapTile = saves[tilesX_Keys[i]];
                tilesX_Temp = tilesX[tilesX_Keys[i]];
                tilesY_Temp = tilesY[tilesY_Keys[i]];
                for (int j = 0; j < tilesX_Temp.Count; j++)
                {
                    coordinates.x = tilesX_Temp[j];
                    coordinates.y = tilesY_Temp[j];
                    tileMap.SetTile(coordinates, mapTile);
                }
            }else if (tileUND.ContainsKey(tilesX_Keys[i]))
            {
                mapTile = tileUND[tilesX_Keys[i]];
                tilesX_Temp = tilesX[tilesX_Keys[i]];
                tilesY_Temp = tilesY[tilesY_Keys[i]];
                for (int j = 0; j < tilesX_Temp.Count; j++)
                {
                    coordinates.x = tilesX_Temp[j];
                    coordinates.y = tilesY_Temp[j];
                    tileMap.SetTile(coordinates, mapTile);
                }
            }else if (tileUD.ContainsKey(tilesX_Keys[i]))
            {
                mapTile = tileUD[tilesX_Keys[i]];
                tilesX_Temp = tilesX[tilesX_Keys[i]];
                tilesY_Temp = tilesY[tilesY_Keys[i]];
                for (int j = 0; j < tilesX_Temp.Count; j++)
                {
                    coordinates.x = tilesX_Temp[j];
                    coordinates.y = tilesY_Temp[j];
                    tileMap.SetTile(coordinates, mapTile);
                }
            }
        }
    }
    #endregion
    /// <summary>
    /// Load data to the MapSaveSystem.
    /// </summary>
    /// <param name="data">the game data</param>
    public void LoadMap(GameData data)
    {
        tileMap.ClearAllTiles();
        tilesRegistered=new Dictionary<float, float>(data.tilesRegistered);
        miniMapItem = data.miniMapItem;
        tilesX = new Dictionary<string, List<int>>(data.tilesX);
        tilesY = new Dictionary<string, List<int>>(data.tilesY);
        SetTilesToTilemap();
    }
}