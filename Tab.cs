using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
    public Image[] tabsImages;
    public GameObject[] pages;
    void Start()
    {
        OpenTab(0);
    }

    public void OpenTab(int tabIndex)
    {
        for (int i = 0; i <= tabsImages.Length - 1; i++)
        {
            tabsImages[i].color = Color.white;
            pages[i].SetActive(false);
        }
        tabsImages[tabIndex].color = Color.grey;
        pages[tabIndex].SetActive(true);
    }
}
