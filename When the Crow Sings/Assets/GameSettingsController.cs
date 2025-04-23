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

        GameSettingsModel model = GameSettings.GetModel();

        PlayerPrefs.SetFloat("MasterVolume", model.masterVolume);
        PlayerPrefs.SetFloat("MusicVolume", model.musicVolume);
        PlayerPrefs.SetFloat("AmbienceVolume", model.ambienceVolume);
        PlayerPrefs.SetFloat("SoundFxVolume", model.soundFxVolume);
        PlayerPrefs.SetFloat("VoicesVolume", model.voicesVolume);
        PlayerPrefs.SetInt("PenClick", model.penClick ? 1 : 0);

        PlayerPrefs.SetInt("AutoRun", model.autoRun ? 1 : 0);
        PlayerPrefs.SetInt("QteSpeed", model.qteSpeed);
        PlayerPrefs.SetInt("QteDecay", model.qteDecay ? 1 : 0);
        PlayerPrefs.SetFloat("TextSpeed", model.textSpeed);
        PlayerPrefs.SetFloat("TextSize", model.textSize);

        PlayerPrefs.SetInt("GraphicsQualityIndex", model.graphicsQualityIndex);
        PlayerPrefs.SetInt("ScreenResolutionIndex", model.screenResolutionIndex);
        PlayerPrefs.SetFloat("ScreenBrightness", model.screenBrightness);
        PlayerPrefs.SetInt("FullScreenEnabled", model.fullScreenEnabled ? 1 : 0);

        PlayerPrefs.Save();
    }
}
