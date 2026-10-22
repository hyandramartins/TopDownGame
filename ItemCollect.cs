using UnityEngine;

public class ItemCollect : MonoBehaviour
{   
    private Inventory inventory;
    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                bool addItem = inventory.AddItem(collision.gameObject);

                if (addItem)
                {
                    item.PickUp();
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
