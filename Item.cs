using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    public int amount = 1;

    private TMP_Text amountText;

    private void Awake()
    {
        amountText = GetComponentInChildren<TMP_Text>();
        UpdateQuantityDisplay();
    }

    public void AddToStack(int n = 1)
    {
        amount += n;
        UpdateQuantityDisplay();
    }

    public int RemoveFromStack(int n = 1)
    {
        int removed = Mathf.Min(n, amount);
        amount -= removed;
        UpdateQuantityDisplay();
        return removed;
    }

    public GameObject CloneItem(int newAmount)
    {
        GameObject clone = Instantiate(gameObject);
        Item cloneItem = clone.GetComponent<Item>();
        cloneItem.amount = newAmount;
        cloneItem.UpdateQuantityDisplay();
        return clone;
    }

    public void UpdateQuantityDisplay()
    {
        if(amountText != null)
        {
            amountText.text = amount > 1 ? amount.ToString() : "";
        }
    }

    public virtual void PickUp()
    {
        Sprite ItemImage = GetComponent<Image>().sprite;
        if (ItemPopupController.Instance != null)
        {
            ItemPopupController.Instance.ShowItemPickup(Name, ItemImage);
        }
    }

    public virtual void UseItem()
    {
        Debug.Log("Using item " + Name);
    }
}
