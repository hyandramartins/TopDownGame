using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HotbarController : MonoBehaviour
{
    public GameObject hotbar;
    public GameObject slotPrefab;
    public int slotAmount;

    private ItemDictionary itemDictionary;
    private Key[] hotbarKeys;

    private void Awake()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();

        hotbarKeys = new Key[slotAmount];

        for (int i = 0; i < slotAmount; i++)
        {
            hotbarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    void Update()
    {
        for (int i = 0; i < slotAmount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot (int index)
    {
        Slot slot = hotbar.transform.GetChild(index).GetComponent<Slot>();
        if (slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.UseItem();
        }
    }

    public List<SaveDataInventory> GetHotbarItems()
    {
        List<SaveDataInventory> dataHotbar = new List<SaveDataInventory>();
        foreach (Transform child in hotbar.transform)
        {
            Slot slot = child.GetComponent<Slot>();
            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                //dataInventory.Add(new SaveDataInventory { itemID = item.ID, slotIndex = child.GetSiblingIndex()});
                SaveDataInventory data = new SaveDataInventory();
                data.itemID = item.ID;
                data.slotIndex = child.GetSiblingIndex();
                dataHotbar.Add(data);
            }
        }
        return dataHotbar;
    }

    public void SetHotbarItems(List<SaveDataInventory> saveDataInventory)
    {
        foreach (Transform child in hotbar.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i <= slotAmount - 1; i++)
        {
            Instantiate(slotPrefab, hotbar.transform);
        }
        foreach (SaveDataInventory data in saveDataInventory)
        {
            if (data.slotIndex < slotAmount)
            {
                Slot slot = hotbar.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}
