using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsModel : MonoBehaviour
{
    public static float masterVolume = 1.0f;
    public static float musicVolume = 1.0f;
    public static float ambienceVolume = 1.0f;
    public static float soundFxVolume = 1.0f;
    public static float voicesVolume = 1.0f;

    public static int autoRun = 0;
    public static int qteSpeed = 4;
    public static int qteDecay = 1;
    public static float textSpeed = 1.0f;

    public static int graphicsQualityIndex = 1;
    public static int screenResolutionIndex = 0;
    public static float screenBrightness = 0.4f;

    private void Start()
    {
        
    }

    //public static void ResetPlayerPrefs()
    //{

    //}

    public static void ErasePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    public static void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
}
