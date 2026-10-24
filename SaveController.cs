using UnityEngine;
using System.IO;
using Cinemachine;
using System.Collections.Generic;
using System.Linq;

public class SaveController : MonoBehaviour
{
    private string saveFilePath;

    private InventoryController inventory;
    private HotbarController hotbar;
    private Chest[] chests;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log(saveFilePath);
        InitializeComponents();
        LoadGame();
    }

    private void InitializeComponents()
    {
        inventory = FindAnyObjectByType<InventoryController>();
        hotbar = FindAnyObjectByType<HotbarController>();
        chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.positionPlayer = GameObject.FindWithTag("Player").transform.position;
        saveData.EdgesMap = FindAnyObjectByType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name;
        saveData.saveDataInventory = inventory.GetInventoryItems();
        saveData.saveDataHotbar = hotbar.GetHotbarItems();
        saveData.saveDataChest = GetChestsState();

        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData));
    }

    private List<SaveDataChest> GetChestsState()
    {
        List<SaveDataChest> chestStates = new List<SaveDataChest>();

        foreach(Chest chest in chests)
        {
            SaveDataChest saveDataChest = new SaveDataChest
            {
                chestID = chest.ChestID,
                isOpened = chest.IsOpened
            };
            chestStates.Add(saveDataChest);
        }
        return chestStates;
    }
    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath));
            GameObject.FindWithTag("Player").transform.position = saveData.positionPlayer;
            FindAnyObjectByType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.EdgesMap).GetComponent<PolygonCollider2D>();

            inventory.SetInventoryItems(saveData.saveDataInventory);

            hotbar.SetHotbarItems(saveData.saveDataHotbar);
            LoadChestStates(saveData.saveDataChest);
        }
        else
        { 
            SaveGame();
            inventory.SetInventoryItems(new List<SaveDataInventory>());
            hotbar.SetHotbarItems(new List<SaveDataInventory>());
        }
    }

    private void LoadChestStates(List<SaveDataChest> chestStates)
    {
        foreach(Chest chest in chests)
        {
            SaveDataChest saveDataChest = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);

            if (saveDataChest != null)
            {
                chest.SetOpened(saveDataChest.isOpened);
            }
        }
    }
}
