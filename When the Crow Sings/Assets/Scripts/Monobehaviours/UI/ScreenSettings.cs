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

    public MenuDropdown qualityDropdownMenu;
    public MenuDropdown resolutionDropdownMenu;


    public Slider brightnessSlider;
    [HideInInspector]
    public Volume volume;
    [HideInInspector]
    public LiftGammaGain liftGammaGain;
    public AspectRatioBorders arb;

    private void Start()
    {
        CheckForNullVariables();
        PopulateResolutions();
        
        qualityDropdownMenu.AddDropdownButton("High");
        qualityDropdownMenu.AddDropdownButton("Medium");
        qualityDropdownMenu.AddDropdownButton("Low");

        GetAndSetSavedSettings();

        arb = FindObjectOfType<AspectRatioBorders>();
    }

    private void GetAndSetSavedSettings()
    {
        int savedQuality = PlayerPrefs.GetInt("quality", 2);
        QualitySettings.SetQualityLevel(savedQuality);
        //////////qualityDropdown.value = savedQuality;
        qualityDropdownMenu.SetCurrentlySelectedButton(savedQuality);

        int savedResolution = PlayerPrefs.GetInt("resolution", 1);
        SetResolution(savedResolution);
        //////////resolutionDropdown.value = savedResolution;
        resolutionDropdownMenu.SetCurrentlySelectedButton(savedResolution);

        brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 0.4f);
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
        //////////resolutionDropdown.ClearOptions();
        resolutions.Clear();

        Resolution[] allResolutions = Screen.resolutions;
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

        /////options.Reverse();

        //Add all collected options at once (outside the loop)
        //////////resolutionDropdown.AddOptions(options);
        foreach (string i in options)
        {
            resolutionDropdownMenu.AddDropdownButton(i);
        }

        //Ensure the dropdown updates correctly
        //////////resolutionDropdown.RefreshShownValue(); // i don't think this needs to happen in the rework - Paul

        //Attach listener only once
        //////////resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdownMenu.DropdownMenuButtonPressed.RemoveAllListeners();
        //////////resolutionDropdown.onValueChanged.AddListener(SetResolution);
        resolutionDropdownMenu.DropdownMenuButtonPressed.AddListener(SetResolution);

        //arb.AdjustAspectRatio(); //might be redundant but eh

        if (options.Count <= 0)
        {
            //////////resolutionDropdown.enabled = false; // does this still need to happen in the rework? Maybe... - Paul
        }
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
        PlayerPrefs.SetInt("resolution", _resolutionIndex);
        PlayerPrefs.Save();

        Resolution _resolution = resolutions[_resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen);

        //arb.AdjustAspectRatio();
    }

    public void SetQuality(int _qualityIndex)
    {
        PlayerPrefs.SetInt("quality", _qualityIndex);
        PlayerPrefs.Save();

        QualitySettings.SetQualityLevel(_qualityIndex);
        //////////qualityDropdown.value = _qualityIndex;
        qualityDropdownMenu.SetCurrentlySelectedButton(_qualityIndex);
    }

    public void SetFullscreen(bool _isFullscreen)
    {
        if (_isFullscreen) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else Screen.fullScreenMode = FullScreenMode.Windowed;

        arb.AdjustAspectRatio();
    }
}

