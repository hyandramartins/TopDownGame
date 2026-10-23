using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class ItemDragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform father;
    CanvasGroup canvasGroup;

    public float minDropDistance = 2f;
    public float maxDropDistance = 3f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
                destinySlot.currentItem.transform.SetParent(currentSlot.transform);
                currentSlot.currentItem = destinySlot.currentItem;
                destinySlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                currentSlot.currentItem = null;
            }
            transform.SetParent(destinySlot.transform);
            destinySlot.currentItem = gameObject;
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
            }
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryPageRect = father.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryPageRect, mousePosition);
    }

    void DropItem(Slot father)
    {
        father.currentItem = null;
        //Find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Missing 'Player' tag");
        }
        //Random drop position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;
        //Instantiate drop item
        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        dropItem.GetComponent<BounceEffect>().StartBounce();
        //Destroy the UI one
        Destroy(gameObject);
    }
}
