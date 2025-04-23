using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsModel : MonoBehaviour
{
    public float masterVolume;
    public float musicVolume;
    public float ambienceVolume;
    public float soundFxVolume;
    public float voicesVolume;
    public bool penClick;

    public bool autoRun;
    public int qteSpeed;
    public bool qteDecay;
    public float textSpeed;
    public int textSize;

    public int graphicsQualityIndex;
    public int screenResolutionIndex;
    public float screenBrightness;
    public bool fullScreenEnabled;
}
