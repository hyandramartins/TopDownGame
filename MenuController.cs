using UnityEngine;
using UnityEngine.InputSystem;
public class MenuController : MonoBehaviour
{   
    public GameObject menu;
    void Start()
    {
        menu.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            menu.SetActive(!menu.activeSelf);
        }
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
    }
}
