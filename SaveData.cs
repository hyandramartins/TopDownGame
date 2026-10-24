using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 positionPlayer;
    public string EdgesMap;
    public List<SaveDataInventory> saveDataInventory;
    public List<SaveDataInventory> saveDataHotbar;
    public List<SaveDataChest> saveDataChest;
}

[System.Serializable]

public class SaveDataChest
{
    public string chestID;
    public bool isOpened;
}