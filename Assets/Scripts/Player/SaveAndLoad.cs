using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoad : MonoBehaviour
{
    [SerializeField] PlayerInventory inventory;
    [SerializeField] PlayerHealth health;
    [SerializeField]MapSaveSystem map;
    [SerializeField] Room rooms;
    private GameData data;
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
        data = SaveSystem.LoadPlayerSlot(slotIndex);
        if (data == null)
        {
            return;
        }
        else
        {
            slot=slotIndex;
            dataLoader();
        }
    }
    /// <summary>
    /// Loads all the game data
    /// </summary>
    /// <param name="data">reference to GameData script to load the game data</param>
    private void dataLoader()
    {
        sectorName = data.actualSector;
        SceneManager.LoadSceneAsync(1,LoadSceneMode.Single).completed+=OnCompleted;
        health.LoadHealth(data);
        map.LoadMap(data);
        Boss.defeateds = new List<int>(data.bossesDefeated);
        TimeCounter.SetTimeAfterLoad(data.time);
        inventory.LoadInventory(data);
        MapUpdater.mappers=new List<int>(data.mappers);
    }
    void OnCompleted(AsyncOperation operation){
        
        GameObject room=rooms.LoadRoom(sectorName);
        room.name=sectorName;
        Vector3 position=new Vector3();
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = 0;
        SaveStation.loaded = true;
        transform.position = position;
        Instantiate(room);
         
        GameEvents.enablePlayer.Invoke();
    }
 
}
