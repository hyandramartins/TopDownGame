using UnityEngine;
using System.IO;
using Cinemachine;
public class Save : MonoBehaviour
{
    private string saveFilePath;

    private Inventory inventory;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        inventory = FindAnyObjectByType<Inventory>();
        
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.positionPlayer = GameObject.FindWithTag("Player").transform.position;
        saveData.EdgesMap = FindAnyObjectByType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name;
        saveData.saveDataInventory = inventory.GetInventoryItems();

        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath));
            GameObject.FindWithTag("Player").transform.position = saveData.positionPlayer;
            FindAnyObjectByType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.EdgesMap).GetComponent<PolygonCollider2D>();

            inventory.SetInventoryItems(saveData.saveDataInventory);
        }
        else
        {
            SaveGame();
        }
    }

}
