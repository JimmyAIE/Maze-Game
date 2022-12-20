using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomiseConsole : MonoBehaviour
{
    public RectTransform ConsoleRectTransform;
    public RectTransform viewPortTransform;
    public RectTransform scrollBarTransform;
    public RectTransform inputFieldTransform;
    public RectTransform textTransform;


    [SerializeField] public Vector2 offset = new Vector2(0, 0);
    [SerializeField] public Vector2 consoleSize = new Vector2(300,200);
    private float textBoundaries = 6;

    private void Update()
    {
        ChangeConsoleAppearance();
    }

    public void Awake()
    {

        ChangeConsoleAppearance();

    }

    public void ChangeConsoleAppearance()
    {
        //offset
        ConsoleRectTransform.anchoredPosition = new Vector3(consoleSize.x/2 + offset.x, consoleSize.y/2 + offset.y, 0);

        //consoleSize
        ConsoleRectTransform.sizeDelta = consoleSize;
        viewPortTransform.sizeDelta = consoleSize;  
        scrollBarTransform.anchoredPosition = new Vector3((consoleSize.x - scrollBarTransform.sizeDelta.x) / 2, scrollBarTransform.anchoredPosition.y, 0);
        scrollBarTransform.sizeDelta = new Vector3(scrollBarTransform.sizeDelta.x, consoleSize.y, 0);

        inputFieldTransform.sizeDelta = new Vector3(consoleSize.x, inputFieldTransform.sizeDelta.y, 0);
        inputFieldTransform.anchoredPosition = new Vector3(inputFieldTransform.anchoredPosition.x, (-consoleSize.y - inputFieldTransform.sizeDelta.y) / 2, 0);

        textTransform.sizeDelta = new Vector3(consoleSize.x - textBoundaries - scrollBarTransform.sizeDelta.x , textTransform.sizeDelta.y, 0);



        gameObject.SendMessage("UpdateConsole");
    }

    
}
