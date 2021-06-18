using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{
    [SerializeField] PlayerInventory inventory;
    [SerializeField] PlayerHealth health;
    [SerializeField] CollectorManager collector;
    [SerializeField]MapSaveSystem map;
    [SerializeField] Room rooms;
    private float[] position= new float[3];
    public string sectorName{get;set;}
    public static bool newGame;
    public static int slot{get;set;}
    public void SetPositions(float x, float y, float z)
    {
        position[0] = x;
        position[1] = y;
        position[2] = z;
    }
    public void SavePlayerSlot(int slotIndex) { SaveSystem.SavePlayerSlot(inventory, health,map, position, sectorName, slotIndex); }
    public void LoadPlayerSlot(int slotIndex)
    {
        GameData data = SaveSystem.LoadPlayerSlot(slotIndex);
        if (data == null)
        {
            return;
        }
        else
        {
            slot=slotIndex;
            dataLoader(data);
        }
    }
    /// <summary>
    /// Loads all game data
    /// </summary>
    /// <param name="data">reference to GameData script to load the game data</param>
    private void dataLoader(GameData data)
    {
        sectorName = data.actualSector;
        AsyncOperation operation=SceneManager.LoadSceneAsync(data.scene,LoadSceneMode.Single);
        StartCoroutine(CheckSceneLoaded(operation,data));
        health.LoadHealth(data);
        map.LoadMap(data);
        Boss.defeateds = new List<int>(data.bossesDefeated);
        LoadToCollectorManager(data);
        TimeCounter.SetTimeAfterLoad(data.time);
        inventory.LoadInventory(data);
        MapUpdater.mappers=new List<int>(data.mappers);
    }
    IEnumerator CheckSceneLoaded(AsyncOperation operation,GameData data){
        while(!operation.isDone){
            yield return new WaitForSecondsRealtime(0.05f);
        }
        GameObject room=rooms.LoadRoom(sectorName);
        room.name=sectorName;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = 0;
        SaveStation.recentlyLoad = true;
        transform.position = position;
        Instantiate(room);
         
        GameEvents.enablePlayer.Invoke();
    }
    private void LoadToCollectorManager(GameData data)
    {
        var reserve = data.reserve;
        var items = data.items;
        collector.reserveSearch.Clear();
        for(int i = 0; i < reserve.Count; i++)
        {
            collector.reserveSearch.Add(reserve[i], new ReserveAcquired());
        }
        collector.itemSearch.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            collector.itemSearch.Add(items[i], new ItemAcquired());
        }
    }
}
