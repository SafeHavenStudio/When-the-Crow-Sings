using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour
{
    private List<Resolution> resolutions = new List<Resolution>();
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Slider brightnessSlider;
    public Volume volume;
    public LiftGammaGain liftGammaGain;

    private void Start()
    {
        CheckForNullVariables();
        PopulateResolutions();
        GetAndSetSavedSettings();
    }

    private void GetAndSetSavedSettings()
    {
        int savedQuality = PlayerPrefs.GetInt("quality", 0);
        QualitySettings.SetQualityLevel(savedQuality);
        qualityDropdown.value = savedQuality;

        brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 0f);
    }

    private void CheckForNullVariables()
    {
        volume = FindObjectOfType<Volume>();

        if (volume == null)
        {
            Debug.Log("there's no global volume in the scene");
            return;
        }

        if (volume.profile == null)
        {
            Debug.Log("Global volume has no profile");
            return;
        }

        volume.profile = Instantiate(volume.profile);

        if (volume.profile.TryGet(out liftGammaGain))
        {
            Debug.Log("Successfully retrieved LiftGammaGain!");
        }
        else
        {
            Debug.LogError("Failed to find LiftGammaGain. Ensure the effect is added to your Volume Profile.");
        }
    }

    public void PopulateResolutions()
    {
        //resolutionDropdown.ClearOptions();
        //resolutions.Clear();

        //Resolution[] allResolutions = Screen.resolutions;
        //List<string> options = new List<string>();

        //foreach (Resolution res in allResolutions)
        //{
        //    float aspectRatio = (float)res.width / res.height;
        //    string resString = res.width + " x " + res.height;

        //    //Only include 16:9 resolutions
        //    if (Mathf.Approximately(aspectRatio, 16f / 9f))
        //    {
        //        resolutions.Add(res);
        //        options.Add(resString);
        //    }
        //}

        ////Add all collected options at once (outside the loop)
        //resolutionDropdown.AddOptions(options);

        ////Ensure the dropdown updates correctly
        //resolutionDropdown.RefreshShownValue();

        ////Attach listener only once
        //resolutionDropdown.onValueChanged.RemoveAllListeners();
        //resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }


    public void SetBrightness(float _brightnessIndex)
    {
        if (liftGammaGain != null)
        {
            liftGammaGain.gain.value = new Vector4(_brightnessIndex, _brightnessIndex, _brightnessIndex, _brightnessIndex);
            volume.profile.TryGet(out LiftGammaGain liftGammaGainOverride);
            brightnessSlider.value = _brightnessIndex;
            PlayerPrefs.SetFloat("brightness", brightnessSlider.value);
            PlayerPrefs.Save();
        }
        else
        {
            throw new Exception("LiftGammaGain is null! Ensure the effect is enabled in the Volume Profile.");
        }
    }

    public void SetResolution(int _resolutionIndex)
    {
        Resolution _resolution = resolutions[_resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int _qualityIndex)
    {
        PlayerPrefs.SetInt("quality", _qualityIndex);
        PlayerPrefs.Save();

        QualitySettings.SetQualityLevel(_qualityIndex);
        qualityDropdown.value = _qualityIndex;
    }

    public void SetFullscreen(bool _isFullscreen)
    {
        Screen.fullScreen = _isFullscreen;
    }
}

