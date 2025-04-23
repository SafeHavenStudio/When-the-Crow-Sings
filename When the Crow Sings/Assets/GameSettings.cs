using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private static GameSettings instance;
    [SerializeField] GameSettingsModel model;
    [SerializeField] GameSettingsController controller;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
        }
    }

    public static GameSettingsModel GetModel()
    {
        return instance.model;
    }

    public static GameSettingsController GetController()
    {
        return instance.controller;
    }
}
