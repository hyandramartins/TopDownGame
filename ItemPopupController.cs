using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class ItemPopupController : MonoBehaviour
{
    public static ItemPopupController Instance { get; private set; }
    public GameObject prefabBoxItemPopup;
    public int maxPopups;
    public float durationPopup;
    private readonly Queue<GameObject> activesPopups = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Erro");
            Destroy(gameObject);
        }
    }

    public void ShowItemPickup(string itemName, Sprite itemImage)
    {
        GameObject newPopup = Instantiate(prefabBoxItemPopup, transform);
        newPopup.GetComponentInChildren<TMP_Text>().text = itemName;

        Image itemIcon = newPopup.transform.Find("ItemImage")?.GetComponent<Image>();
        if (itemIcon)
        {
            itemIcon.sprite = itemImage;
        }

        activesPopups.Enqueue(newPopup);
        if(activesPopups.Count > maxPopups)
        {
            Destroy(activesPopups.Dequeue());
        }

        StartCoroutine(FadeOutAndDestroy(newPopup));
    }

    private IEnumerator FadeOutAndDestroy(GameObject popup)
    {
        yield return new WaitForSeconds(durationPopup);
        if (popup == null) yield break;
        
        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for(float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popup == null) yield break;
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }

        Destroy(popup);
    }
}
