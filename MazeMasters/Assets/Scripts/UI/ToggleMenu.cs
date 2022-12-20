using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    public GameObject menu;
    public void Start()
    {
        ControlManager.instance.inGameUIControls.General.ToggleMenu.performed += ctx => MenuToggle();
    }

    public void MenuToggle()
    {
        if (!menu.activeSelf)
        {
            menu.SetActive(true);
            ControlManager.instance.EnableMenuControls();
        }
    }
}
