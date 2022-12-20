using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleConsole : MonoBehaviour
{

    public GameObject console;


    public void Start()
    {
        ControlManager.instance.inGameUIControls.General.StartTypingConsole.performed += ctx => OnStartTypingConsole();
        ControlManager.instance.inGameUIControls.General.ToggleConsole.performed += ctx => OnToggleConsole();
    }

    public void OnToggleConsole()
    {
        if (console.activeSelf == true)
        {
            console.SetActive(false);
        }
        else
        {
            console.SetActive(true);
        }
    }

    public void OnStartTypingConsole()
    {
        if (!console.activeSelf)
        {
            console.SetActive(true);
            console.SendMessage("OnStartTypingConsole");
        }
    }

}
