using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SettingsMenu : MonoBehaviour
{
    public GameObject menuDecorations;
    public RectTransform viewPort;
    public RectTransform contentTransform;
    public Scrollbar scrollbar;
    public float scrollSens;

    public void Start()
    {
        ControlManager.instance.inGameUIControls.Settings.CloseSettings.performed += ctx => CloseSettings();
        ControlManager.instance.inGameUIControls.Settings.croll.performed += ctx => Scroll(ctx);
        scrollbar.size = viewPort.sizeDelta.y / contentTransform.sizeDelta.y;
        scrollbar.value = 0;
        ScrollContent();
    }

    public void Scroll(InputAction.CallbackContext ctx)
    {
        ScrollboxPosition.ScrollContentPosition(ctx.ReadValue<float>(), scrollSens, viewPort, contentTransform, scrollbar, true);
    }

    public void CloseSettings()
    {
        ControlManager.instance.DisableSettingsControls();
        menuDecorations.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ScrollContent()
    {
        ScrollboxPosition.AdjustContentPosition(viewPort, contentTransform, scrollbar, true);
    }
}
