using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menuDecorations;
    public GameObject settings;



    private void Start()
    {
        Debug.Log("Menu Start");
        ControlManager.instance.inGameUIControls.Menu.CloseMenu.performed += ctx => CloseMenu();
    }

    public void ExitGame()
    {
        Debug.Log("quitting");
        Application.Quit();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        ControlManager.instance.DisableMenuControls();
    }
    

    public void ToggleSettings()
    {
        if (settings.activeSelf)
        {
            settings.SetActive(false);
            menuDecorations.SetActive(true);
            ControlManager.instance.EnableMenuControls();
        }
        else
        {
            settings.SetActive(true);
            menuDecorations.SetActive(false);
            ControlManager.instance.EnableSettingsControls();
        }
    }
}
