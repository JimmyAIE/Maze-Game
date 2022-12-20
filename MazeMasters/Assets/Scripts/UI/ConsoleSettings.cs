using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleSettings : MonoBehaviour
{
    public TMP_InputField offsetX;
    public TMP_InputField offsetY;

    public TMP_InputField sizeX;
    public TMP_InputField sizeY;

    Console console;
    CustomiseConsole consoleCustomiser;


    public void Awake()
    {
        GameManager.updateSettings += ChangeConsoleAppearance;
        console = GameObject.FindObjectOfType<Console>(true);
        consoleCustomiser = console.GetComponent<CustomiseConsole>();
        offsetX.text = consoleCustomiser.offset.x.ToString();
        offsetY.text = consoleCustomiser.offset.y.ToString();
        sizeX.text = consoleCustomiser.consoleSize.x.ToString();
        sizeY.text = consoleCustomiser.consoleSize.y.ToString();
    }

    public void ChangeConsoleAppearance()
    {
        consoleCustomiser.offset = new Vector2(int.Parse(offsetX.text), int.Parse(offsetY.text));
        consoleCustomiser.consoleSize = new Vector2(int.Parse(sizeX.text), int.Parse(sizeY.text));
        console.gameObject.SetActive(true);
        console.gameObject.SendMessage("ChangeConsoleAppearance");
        console.gameObject.SetActive(false);
    }
}
