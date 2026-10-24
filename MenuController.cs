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
            if(!menu.activeSelf && PauseController.IsGamePaused)
            {
                return;
            }
            menu.SetActive(!menu.activeSelf);
            PauseController.SetPause(menu.activeSelf);
        }
    }

    public void CloseMenu()
    {
        menu.SetActive(false);
        PauseController.SetPause(false);
    }
}
