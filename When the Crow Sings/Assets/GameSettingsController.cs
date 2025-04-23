using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsController : MonoBehaviour
{

    private void Start()
    {
        LoadPlayerPrefs();
    }

    void LoadPlayerPrefs()
    {
        Debug.Log("Loading preferences.");
        GameSettingsModel model = GameSettings.GetModel();
        model.masterVolume = PlayerPrefs.GetFloat("MasterVolume", model.masterVolume);
        model.musicVolume = PlayerPrefs.GetFloat("MusicVolume", model.musicVolume);
        model.ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", model.ambienceVolume);
        model.soundFxVolume = PlayerPrefs.GetFloat("SoundFxVolume", model.soundFxVolume);
        model.voicesVolume = PlayerPrefs.GetFloat("VoicesVolume", model.voicesVolume);

        model.autoRun = PlayerPrefs.GetInt("AutoRun", model.autoRun);
        model.qteSpeed = PlayerPrefs.GetInt("QteSpeed", model.qteSpeed);
        model.qteDecay = PlayerPrefs.GetInt("QteDecay", model.qteDecay);
        model.textSpeed = PlayerPrefs.GetFloat("TextSpeed", model.textSpeed);

        model.graphicsQualityIndex = PlayerPrefs.GetInt("GraphicsQualityIndex", model.graphicsQualityIndex);
        model.screenResolutionIndex = PlayerPrefs.GetInt("ScreenResolutionIndex", model.screenResolutionIndex);
        model.screenBrightness = PlayerPrefs.GetFloat("ScreenBrightness", model.screenBrightness);

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
