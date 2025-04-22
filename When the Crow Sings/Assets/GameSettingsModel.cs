using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsModel : MonoBehaviour
{
    public float masterVolume = 1.0f;
    public float musicVolume = 1.0f;
    public float ambienceVolume = 1.0f;
    public float soundFxVolume = 1.0f;
    public float voicesVolume = 1.0f;

    public int autoRun = 0;
    public int qteSpeed = 4;
    public int qteDecay = 1;
    public float textSpeed = 1.0f;

    public int graphicsQualityIndex = 1;
    public int screenResolutionIndex = 0;
    public float screenBrightness = 0.4f;

    public void ErasePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    public void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
}
