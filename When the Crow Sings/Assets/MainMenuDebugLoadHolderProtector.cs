using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuDebugLoadHolderProtector : MonoBehaviour
{
    public MainMenuDebugLoadHolder mainMenuDebugLoadHolder;
    private static MainMenuDebugLoadHolderProtector instance;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
