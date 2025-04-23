using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingsController : MonoBehaviour
{

    private void Start()
    {
        //PlayerPrefs.SetFloat("ExamplePref", 1.5f);
        SavePlayerPrefs();
        //Debug.Log("ExamplePref exists == " + PlayerPrefs.HasKey("ExamplePref"));
        Debug.Log("ExamplePref == " + PlayerPrefs.GetFloat("ExamplePrefferentnythhsgjk").ToString());
    }

    void LoadPlayerPrefs()
    {
        //        public float masterVolume = 1.0f;
        //public float musicVolume = 1.0f;
        //public float ambienceVolume = 1.0f;
        //public float soundFxVolume = 1.0f;
        //public float voicesVolume = 1.0f;

        //public int autoRun = 0;
        //public int qteSpeed = 4;
        //public int qteDecay = 1;
        //public float textSpeed = 1.0f;

        //public int graphicsQualityIndex = 1;
        //public int screenResolutionIndex = 0;
        //public float screenBrightness = 0.4f;
        GameSettings.GetModel().masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        GameSettings.GetModel().musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        GameSettings.GetModel().ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume", 1.0f);
        GameSettings.GetModel().soundFxVolume = PlayerPrefs.GetFloat("SoundFxVolume", 1.0f);
        GameSettings.GetModel().voicesVolume = PlayerPrefs.GetFloat("VoicesVolume", 1.0f);
        GameSettings.GetModel().masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        GameSettings.GetModel().masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        GameSettings.GetModel().masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        GameSettings.GetModel().masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        GameSettings.GetModel().masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);

    }

    public void ErasePlayerPrefs()
    {
        Debug.Log("Erasing preferences.");
        PlayerPrefs.DeleteAll();
        Debug.Log("ExamplePref exists == " + PlayerPrefs.HasKey("ExamplePref"));
    }
    public void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
}
