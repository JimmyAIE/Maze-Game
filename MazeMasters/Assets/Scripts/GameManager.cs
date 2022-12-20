using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public delegate void UpdateSettings();
    public static event UpdateSettings updateSettings;


    private void Awake()
    {

        if (instance != null)
        {
            GameObject.Destroy(this);
        }
        instance = this;
    }

    public void ApplySettings()
    {
        if (updateSettings != null)
        {
            updateSettings();
        }
    }
}
