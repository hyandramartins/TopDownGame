using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance {get; private set;}

    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image FaceImage;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

    void Awake()
    {
        if(Instance == null) Instance =  this;
        else Destroy(gameObject);
    }

    public void ShowDialogueIU(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite face)
    {
        nameText.text = npcName;
        FaceImage.sprite = face;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach(Transform child in choiceContainer) Destroy(child.gameObject);
    }

    public void CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
    }
}
