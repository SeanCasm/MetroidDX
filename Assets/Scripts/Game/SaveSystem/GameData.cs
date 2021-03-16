using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData 
{
    public int tanks, health, scene;
    public float[] position;
    public string actualSector;
    public List<int> reserve, items;
    public Dictionary<int, int> ammoMunition=new Dictionary<int, int>();//id and max munition
    public Dictionary<int, bool> selectItems=new Dictionary<int, bool>();//id and select
    public Dictionary<string, List<int>> tilesX, tilesY;
    public List<int> bossesDefeated;//boss id and name
    public Dictionary<float,float> tilesRegistered=new Dictionary<float, float>();
    public bool[] miniMapItem = new bool[40];
    public GameData(PlayerInventory inventory,PlayerHealth energy,MapSaveSystem map,float[] pos,string sectorName)
    {
        actualSector = sectorName;
        var limitedAmmo = inventory.limitedAmmo;
        for (int i = 0; i < limitedAmmo.Count; i++)
        {
            if (limitedAmmo[i].maxAmmo > 0)
            {
                ammoMunition.Add(limitedAmmo[i].iD,limitedAmmo[i].maxAmmo);
            }
        }
        scene = SceneManager.GetActiveScene().buildIndex;
        reserve = new List<int>(inventory.reserve);
        items = new List<int>(inventory.items);
        SelectItemsData(inventory);
        position = new float[2];
        bossesDefeated=new List<int>(Boss.defeateds);
        tanks = energy.Tanks;
        position[0] = pos[0];
        position[1] = pos[1];
        tilesX =new Dictionary<string, List<int>> ( map.tilesX);
        tilesY =new Dictionary<string, List<int>> (map.tilesY);
        tilesRegistered = new Dictionary<float, float>(map.tilesRegistered);
        miniMapItem = map.miniMapItem;
    }
    void SelectItemsData(PlayerInventory inventory)
    {
        var items = inventory.playerItems;
        foreach (var element in items)
        {
            selectItems.Add(element.Key, element.Value.selected);
        }
    }
}
[System.Serializable]
public class GlobalGameData{
    public float fXVolume, musicVolume;
    public string keyBindings;
    public GlobalGameData(float fx,float music)
    {
        fXVolume=fx;
        musicVolume=music;
    }
}