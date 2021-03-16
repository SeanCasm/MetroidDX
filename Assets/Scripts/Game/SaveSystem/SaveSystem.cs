
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static GlobalGameData LoadSettings()
    {
        string path = Application.persistentDataPath + "/config.globalxd";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GlobalGameData data = formatter.Deserialize(stream) as GlobalGameData;
            stream.Close();
            return data;
        }
        else
        {

            return null;
        }
    }
    public static void SaveSettings(float volumeLevel,float musicLevel)
    {
        string slotPath = "/config.globalxd";
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + slotPath;
        FileStream stream = new FileStream(path, FileMode.Create);
        GlobalGameData data = new GlobalGameData(volumeLevel, musicLevel);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static void SavePlayerSlot(PlayerInventory inventory, PlayerHealth energy,
        MapSaveSystem map,float[] pos, string sectorName, int slotIndex)
    {
        string slotPath = GetSlotPath(slotIndex);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + slotPath;
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData data = new GameData(inventory, energy,map, pos, sectorName);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    private static string GetSlotPath(int slotIndex)
    {
        switch (slotIndex)
        {
            case 0:
                return "/player.xd1";
            case 1:
                return "/player.xd2";
            case 2:
                return "/player.xd3";
        }
        return null;
    }
    public static GameData LoadPlayerSlot(int slotIndex)
    {

        string path = Application.persistentDataPath + GetSlotPath(slotIndex);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
    public static void ErasePlayerSlot(int slotIndex)
    {
        System.IO.File.Delete(Application.persistentDataPath + GetSlotPath(slotIndex));
    }
}
