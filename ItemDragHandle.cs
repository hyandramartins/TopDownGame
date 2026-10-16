using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform father;
    CanvasGroup canvasGroup;

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
            transform.SetParent(father);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

    }
}
