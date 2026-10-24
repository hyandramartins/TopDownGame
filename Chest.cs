using UnityEngine;

public class Chest : MonoBehaviour, InteractableInterface
{   
    public bool IsOpened {get; private set;}
    public string ChestID {get; private set;}
    public GameObject itemPrefab;
    public Sprite openedSprite;
    void Start()
    {
        ChestID ??= Global.GenerateUniqueID(gameObject);
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interact()
    {
        if(!CanInteract()) return;
        OpenChest();
        
    }

    private void OpenChest()
    {
        //SetOpened
        SetOpened(true);

        //DropItem
        if (itemPrefab)
        {
            GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.down, Quaternion.identity);
            droppedItem.GetComponent<BounceEffect>().StartBounce();
        }
    }

    public void SetOpened(bool opened)
    {
        //IsOpened = opened;
        if (IsOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }
}
