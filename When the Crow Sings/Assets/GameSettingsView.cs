using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsView : MonoBehaviour
{
    public MenuDropdown textSizeDropdownMenuPopup;
    public Slider qteSpeedSlider;
    public Slider textSpeedSlider;
    public Toggle qteDecayToggle;
    public Toggle autoRunToggle;

    public MenuDropdown resolutionDropdownMenuPopup;
    public MenuDropdown graphicsQualityDropdownMenuPopup;
    public Slider brightnessSlider;
    public Toggle fullScreenToggle;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider ambienceVolumeSlider;
    public Slider soundFxVolumeSlider;
    public Slider voicesVolumeSlider;
    public Toggle penClickSoundToggle;

    // TODO: whatever's going on with the text style struct in the original script.

    // The following are only here for completion's sake while refactoring. Will not be used.
    //public List<TimingMeterQTE> timingMeterQtePrefabs;
    //public List<StirringQTE> stirringQtePrefabs;
    //public List<TextMeshProUGUI> dialogueText;

    public void UpdateViewToModel()
    {
        GameSettingsModel model = GameSettings.GetModel();

        textSizeDropdownMenuPopup.SetCurrentlySelectedButton(model.textSize);
        qteSpeedSlider.value = model.qteSpeed;
        textSpeedSlider.value = model.textSpeed;
        autoRunToggle.isOn = model.autoRun;
        qteDecayToggle.isOn = model.qteDecay;

        resolutionDropdownMenuPopup.SetCurrentlySelectedButton(model.textSize);
        graphicsQualityDropdownMenuPopup.SetCurrentlySelectedButton(model.textSize);
        brightnessSlider.value = model.screenBrightness;
        fullScreenToggle.isOn = model.fullScreenEnabled;

        masterVolumeSlider.value = model.masterVolume;
        musicVolumeSlider.value = model.musicVolume;
        ambienceVolumeSlider.value = model.ambienceVolume;
        soundFxVolumeSlider.value = model.soundFxVolume;
        voicesVolumeSlider.value = model.voicesVolume;
        penClickSoundToggle.isOn = model.penClick;
    }


    private void Start()
    {
        PopulateDropdowns_Legacy();
        ConnectViewCallbacksToModel();
        UpdateViewToModel();
    }

    void PopulateDropdowns_Legacy()
    {
        // Set text size buttons (legacy code)

        List<string> textSizes = new List<string>() { "Default", "Large" };
        foreach (string i in textSizes)
        {
            textSizeDropdownMenuPopup.AddDropdownButton(i);
        }


        // Set graphics (legacy code)

        graphicsQualityDropdownMenuPopup.AddDropdownButton("High");
        graphicsQualityDropdownMenuPopup.AddDropdownButton("Medium");
        graphicsQualityDropdownMenuPopup.AddDropdownButton("Low");


        // Set resolutions (legacy code)

        List<Resolution> resolutions = new List<Resolution>();
        resolutions.Clear();
        Resolution[] allResolutions = Screen.resolutions.Reverse().ToArray(); //This should reverse the order of which they populate
        List<string> options = new List<string>();
        HashSet<string> uniqueResolutions = new HashSet<string>(); //ensures no duplicates are allowed

        foreach (Resolution res in allResolutions)
        {
            float aspectRatio = (float)res.width / res.height;
            string resString = res.width + " x " + res.height;

            //Only include 16:9 resolutions and skip duplicates
            if (Mathf.Approximately(aspectRatio, 16f / 9f) && !uniqueResolutions.Contains(resString))
            {
                uniqueResolutions.Add(resString);
                resolutions.Add(res);
                options.Add(resString);
            }
        }

        foreach (string i in options)
        {
            resolutionDropdownMenuPopup.AddDropdownButton(i);
        }
    }

    void ConnectViewCallbacksToModel()
    {
        qteSpeedSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().qteSpeed = (int)newValue;
            ViewPrint("QTE Speed set to " + GameSettings.GetModel().qteSpeed.ToString());
        });

        textSpeedSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().textSpeed = newValue;
            ViewPrint("Text Speed set to " + GameSettings.GetModel().textSpeed.ToString());
        });

        autoRunToggle.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().autoRun = newValue;
            ViewPrint("Auto-Run toggled to " + GameSettings.GetModel().autoRun.ToString());
        });

        qteDecayToggle.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().qteDecay = newValue;
            ViewPrint("QTE Decay toggled to " + GameSettings.GetModel().qteDecay.ToString());
        });

        textSizeDropdownMenuPopup.DropdownMenuButtonPressed.AddListener((newValue) => {
            GameSettings.GetModel().textSize = newValue;
            ViewPrint("TextSize set to " + GameSettings.GetModel().textSize.ToString());
        });

        resolutionDropdownMenuPopup.DropdownMenuButtonPressed.AddListener((newValue) => {
            GameSettings.GetModel().screenResolutionIndex = newValue;
            ViewPrint("ScreenResolutionIndex set to " + GameSettings.GetModel().screenResolutionIndex.ToString());
        });

        graphicsQualityDropdownMenuPopup.DropdownMenuButtonPressed.AddListener((newValue) => {
            GameSettings.GetModel().graphicsQualityIndex = newValue;
            ViewPrint("GraphicsQualityIndex set to " + GameSettings.GetModel().graphicsQualityIndex.ToString());
        });

        brightnessSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().screenBrightness = newValue;
            ViewPrint("Brightness set to " + GameSettings.GetModel().screenBrightness.ToString());
        });

        fullScreenToggle.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().fullScreenEnabled = newValue;
            ViewPrint("Fullscreen toggled to " + GameSettings.GetModel().fullScreenEnabled.ToString());
        });

        masterVolumeSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().masterVolume = newValue;
            ViewPrint("Master Volume set to " + GameSettings.GetModel().masterVolume.ToString());
        });
        musicVolumeSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().musicVolume = newValue;
            ViewPrint("Music Volume set to " + GameSettings.GetModel().musicVolume.ToString());
        });
        ambienceVolumeSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().ambienceVolume = newValue;
            ViewPrint("Ambiene Volume set to " + GameSettings.GetModel().ambienceVolume.ToString());
        });
        soundFxVolumeSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().soundFxVolume = newValue;
            ViewPrint("SoundFX Volume set to " + GameSettings.GetModel().soundFxVolume.ToString());
        });
        voicesVolumeSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().voicesVolume = newValue;
            ViewPrint("Voices Volume set to " + GameSettings.GetModel().voicesVolume.ToString());
        });
        penClickSoundToggle.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().penClick = newValue;
            ViewPrint("PenClick toggled to " + GameSettings.GetModel().penClick.ToString());
        });
    }

    void ViewPrint(string message)
    {
        Debug.Log(message);
    }
}
