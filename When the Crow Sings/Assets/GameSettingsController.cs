using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsController : MonoBehaviour
{

    private void Start()
    {
        LoadPlayerPrefs();
    }

    public void LoadPlayerPrefs()
    {   
        Debug.Log("Loading preferences.");
        GameSettingsModel model = GameSettings.GetModel();

        model.masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        model.musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        model.ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", 1.0f);
        model.soundFxVolume = PlayerPrefs.GetFloat("SoundFxVolume", 1.0f);
        model.voicesVolume = PlayerPrefs.GetFloat("VoicesVolume", 1.0f);
        model.penClick = PlayerPrefs.GetInt("PenClick", 1) == 1;

        model.autoRun = PlayerPrefs.GetInt("AutoRun", 1) == 1;
        model.qteSpeed = PlayerPrefs.GetInt("QteSpeed", 4);
        model.qteDecay = PlayerPrefs.GetInt("QteDecay", 1) == 1;
        model.textSpeed = PlayerPrefs.GetFloat("TextSpeed", 0.1f);
        model.textSize = PlayerPrefs.GetInt("TextSize", 1);

        model.graphicsQualityIndex = PlayerPrefs.GetInt("GraphicsQualityIndex", 0);
        model.screenResolutionIndex = PlayerPrefs.GetInt("ScreenResolutionIndex", 0);
        model.screenBrightness = PlayerPrefs.GetFloat("ScreenBrightness", 0.4f);
        model.fullScreenEnabled = PlayerPrefs.GetInt("FullScreenEnabled", 1) == 1;
    }

    public void ErasePlayerPrefs()
    {
        Debug.Log("Erasing preferences.");
        PlayerPrefs.DeleteAll();
    }
    public void SavePlayerPrefs()
    {
        Debug.Log("Saving preferences.");
        PlayerPrefs.Save();
    }
}
