using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ItemDragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Transform father;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    private InventoryController inventoryController;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventoryController = InventoryController.Instance;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        father = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot destinySlot = eventData.pointerEnter?.GetComponent<Slot>();

        if (destinySlot == null)
        {
            GameObject item = eventData.pointerEnter;
            if (item != null)
            {
                destinySlot = item.GetComponentInParent<Slot>();
            }
        }

        Slot currentSlot = father.GetComponent<Slot>();

        if (destinySlot != null)
        {
            if (destinySlot.currentItem != null)
            {
                Item draggedItem = GetComponent<Item>();
                Item targetItem = destinySlot.currentItem.GetComponent<Item>();

                if (draggedItem.ID == targetItem.ID)
                {
                    targetItem.AddToStack(draggedItem.amount);
                    currentSlot.currentItem = null;
                    Destroy(gameObject);
                }
                else
                {
                    //Slot has an item then swap itens
                    destinySlot.currentItem.transform.SetParent(currentSlot.transform);
                    currentSlot.currentItem = destinySlot.currentItem;
                    destinySlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    transform.SetParent(destinySlot.transform);
                    destinySlot.currentItem = gameObject;
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }

            }
            else
            {
                currentSlot.currentItem = null;
                transform.SetParent(destinySlot.transform);
                destinySlot.currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }

        }
        else
        {
            if (!IsWithinInventory(eventData.position))
            {
                //Drop our item
                DropItem(currentSlot);
            }
            else
            {   //Snap back to og slot
                transform.SetParent(father);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
        }
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryPageRect = father.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryPageRect, mousePosition);
    }

    void DropItem(Slot originalSlot)
    {
        Item item = GetComponent<Item>();
        int amount = item.amount;

        if (amount > 1)
        {
            item.RemoveFromStack();

            transform.SetParent(father);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            amount = 1;
        }
        else
        {
            originalSlot.currentItem = null;
        }

        //Find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }
        //Random drop position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;
        //Instantiate drop item
        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        Item droppedItem = dropItem.GetComponent<Item>();
        droppedItem.amount = 1;
        dropItem.GetComponent<BounceEffect>().StartBounce();
        //Destroy the UI one
        if (amount <= 1 && originalSlot.currentItem == null)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            SpliteStack();
        }
    }

    private void SpliteStack()
    {
        Item item = GetComponent<Item>();
        if (item == null || item.amount <= 1) return;

        int splitAmount = item.amount / 2;
        if (splitAmount <= 0) return;

        item.RemoveFromStack(splitAmount);

        GameObject newItem = item.CloneItem(splitAmount);

        if (inventoryController == null || newItem == null) return;

        foreach (Transform slotTransform in inventoryController.inventoryPage.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                slot.currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                return;
            }
        }
        //No empty slot then return to stack
        item.AddToStack(splitAmount);
        Destroy(newItem);
    }
}
