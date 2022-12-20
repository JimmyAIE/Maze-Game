using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Console : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text messageDisplay;
    public RectTransform textObject;
    public RectTransform viewPort;
    public Scrollbar scrollbar;



    public List<string> consoleMessages = new List<string>();


    public float scrollSens;

    private void Awake()
    {
        messageDisplay.text = "";
        scrollbar.size = 1;
    }

    public void Start()
    {
        //controls
        ControlManager.instance.inGameUIControls.Console.Submit.performed += ctx => OnSubmit();
        ControlManager.instance.inGameUIControls.Console.DeselectConsole.performed += ctx => OnDeselectConsole();
        ControlManager.instance.inGameUIControls.General.StartTypingConsole.performed += ctx => OnStartTypingConsole();
        ControlManager.instance.inGameUIControls.Console.Scroll.performed += ctx => OnScroll(ctx);
    }

    public void OnStartTypingConsole()
    {
        ControlManager.instance.EnableConsoleControls();
        inputField.ActivateInputField();
        inputField.Select();
    }

    public void OnDeselectConsole()
    {
        inputField.ReleaseSelection();
        ControlManager.instance.DisableConsoleControls();
        inputField.ReleaseSelection();
    }


    public void OnSubmit()
    {

        if (inputField.text == string.Empty)
        {
            if (inputField.isFocused)
            {
                OnDeselectConsole();
            }
            else
            {
                OnStartTypingConsole();
            }
            return;
        }

        DetermineMessageType();

        inputField.text = "";

        inputField.ActivateInputField();
        inputField.Select();
    }

    public void DetermineMessageType()
    {
        if (false)
        {

        }
        else
        {
            AddToConsole(inputField.text);
        }
    }

    public void AddToConsole(string str)
    {
        consoleMessages.Add(str);
        UpdateConsole();
    }

    public void UpdateConsole()
    {
        messageDisplay.text = string.Empty;
        foreach (string str in consoleMessages)
        {
            messageDisplay.text += str + "\n";
        }
        Canvas.ForceUpdateCanvases();

        scrollbar.size = viewPort.sizeDelta.y / textObject.sizeDelta.y;
        if (scrollbar.size == 0)
        {
            scrollbar.size = 1;
        }

        if (scrollbar.value == 0)
        {
            ScrollText();
        }
    }

    public void ScrollText()
    {
        ScrollboxPosition.AdjustContentPosition(viewPort, textObject, scrollbar, false);
    }

    public void OnScroll(InputAction.CallbackContext ctx)
    {
        ScrollboxPosition.ScrollContentPosition(ctx.ReadValue<float>(), scrollSens, viewPort, textObject, scrollbar, false);
    }
}
