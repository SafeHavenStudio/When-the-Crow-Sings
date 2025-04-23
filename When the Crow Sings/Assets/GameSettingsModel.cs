using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsModel : MonoBehaviour
{
    // Default values are used as default values in PlayerPrefs API in the GameSettingsController.
    public float masterVolume = 1.0f;
    public float musicVolume = 1.0f;
    public float ambienceVolume = 1.0f;
    public float soundFxVolume = 1.0f;
    public float voicesVolume = 1.0f;

    public int autoRun = 0;
    public int qteSpeed = 4;
    public int qteDecay = 1;
    public float textSpeed = 1.0f;

    public int graphicsQualityIndex = 2;
    public int screenResolutionIndex = 0;
    public float screenBrightness = 0.4f;
}
