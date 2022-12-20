using UnityEngine;

public class ControlManager : MonoBehaviour
{

    public static ControlManager instance;


    [HideInInspector]
    public InGameUI inGameUIControls;

    private void Awake()
    {

        inGameUIControls = new InGameUI();
        if (instance != null)
        {
            GameObject.Destroy(this);
        }
        instance = this;
    }

    private void Start()
    {
        inGameUIControls.Console.Disable();
        inGameUIControls.General.Enable();
        inGameUIControls.Menu.Disable();
        inGameUIControls.Settings.Disable();
    }

    public void EnableMenuControls()
    {
        ControlManager.instance.inGameUIControls.General.Disable();
        ControlManager.instance.inGameUIControls.Menu.Enable();
    }

    public void DisableMenuControls()
    {
        ControlManager.instance.inGameUIControls.General.Enable();
        ControlManager.instance.inGameUIControls.Menu.Disable();
    }

    public void EnableConsoleControls()
    {
        inGameUIControls.Console.Enable();
        inGameUIControls.General.Disable();
    }

    public void DisableConsoleControls()
    {
        inGameUIControls.Console.Disable();
        inGameUIControls.General.Enable();
    }

    public void EnableSettingsControls()
    {
        inGameUIControls.Menu.Disable();
        inGameUIControls.Settings.Enable();
    }

    public void DisableSettingsControls()
    {
        inGameUIControls.Menu.Enable();
        inGameUIControls.Settings.Disable();
    }


}
