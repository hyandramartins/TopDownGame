using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; }
    private ItemDictionary itemDictionary;

    public GameObject inventoryPage;
    public GameObject slotPrefab;
    public int slotAmount;

    public GameObject[] itemsPrefabs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {

        itemDictionary = FindAnyObjectByType<ItemDictionary>();

        // for(int i = 0; i <= slotAmount - 1; i++)
        // {
        //     Slot slot = Instantiate(slotPrefab, inventoryPage.transform).GetComponent<Slot>();
        //     if (i < itemsPrefabs.Length)
        //     {
        //         GameObject item = Instantiate(itemsPrefabs[i], slot.transform);
        //         item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //         slot.currentItem = item;
        //     }
        // }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        Item itemAdd = itemPrefab.GetComponent<Item>();
        if (itemAdd == null) return false;

        foreach (Transform child in inventoryPage.transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();
                if (slotItem != null && slotItem.ID == itemAdd.ID)
                {
                    //Same item, stack them
                    slotItem.AddToStack();
                    return true;
                }
            }
        }

        foreach (Transform child in inventoryPage.transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject item = Instantiate(itemPrefab, child);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = item;
                return true;
            }
        }
        Debug.Log("Full inventory!!");
        return false;
    }

    public List<SaveDataInventory> GetInventoryItems()
    {
        List<SaveDataInventory> dataInventory = new List<SaveDataInventory>();
        foreach (Transform child in inventoryPage.transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                //dataInventory.Add(new SaveDataInventory { itemID = item.ID, slotIndex = child.GetSiblingIndex()});
                SaveDataInventory data = new SaveDataInventory();
                data.itemID = item.ID;
                data.slotIndex = child.GetSiblingIndex();
                data.amount = item.amount;
                dataInventory.Add(data);
            }
        }
        return dataInventory;
    }

    public void SetInventoryItems(List<SaveDataInventory> saveDataInventory)
    {
        foreach (Transform child in inventoryPage.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i <= slotAmount - 1; i++)
        {
            Instantiate(slotPrefab, inventoryPage.transform);
        }
        foreach (SaveDataInventory data in saveDataInventory)
        {
            if (data.slotIndex < slotAmount)
            {
                Slot slot = inventoryPage.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    Item itemComponent = item.GetComponent<Item>();
                    if (itemComponent != null && data.amount > 1)
                    {
                        itemComponent.amount = data.amount;
                        itemComponent.UpdateQuantityDisplay();
                    }

                    slot.currentItem = item;
                }
            }
        }
    }
}
