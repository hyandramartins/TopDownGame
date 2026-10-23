using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;

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
