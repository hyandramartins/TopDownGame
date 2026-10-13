using UnityEngine;
using System.IO;
using Cinemachine;
public class Save : MonoBehaviour
{
    private string saveFilePath;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        saveData.positionPlayer = GameObject.FindWithTag("Player").transform.position;
        saveData.EdgesMap = FindAnyObjectByType<CinemachineConfiner>().m_BoundingShape2D.gameObject.name;

        File.WriteAllText(saveFilePath, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath));
            GameObject.FindWithTag("Player").transform.position = saveData.positionPlayer;
            FindAnyObjectByType<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find(saveData.EdgesMap).GetComponent<PolygonCollider2D>();
        }
        else
        {
            SaveGame();
        }
    }

}
