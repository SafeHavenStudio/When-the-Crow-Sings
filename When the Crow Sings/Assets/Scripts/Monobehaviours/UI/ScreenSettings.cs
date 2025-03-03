using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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

        populateResolutions();

        int savedQuality = PlayerPrefs.GetInt("quality", 0);
        QualitySettings.SetQualityLevel(savedQuality);
        qualityDropdown.value = savedQuality;

        brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 0f);
    }

    public void populateResolutions()
    {
        resolutionDropdown.ClearOptions();
        resolutions.Clear();

        Resolution[] allResolutions = Screen.resolutions;
        List<string> options = new List<string>();

        foreach (Resolution res in allResolutions)
        {
            float aspectRatio = (float)res.width / res.height;
            string resString = res.width + " x " + res.height;

            //Only include 16:9 resolutions
            if (Mathf.Approximately(aspectRatio, 16f / 9f))
            {
                resolutions.Add(res);
                options.Add(resString);
            }
        }

        //Add all collected options at once (outside the loop)
        resolutionDropdown.AddOptions(options);

        //Ensure the dropdown updates correctly
        resolutionDropdown.RefreshShownValue();

        //Attach listener only once
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(setResolution);
    }


    public void setBrightness(float brightnessIndex)
    {
        if (liftGammaGain != null)
        {
            liftGammaGain.gain.value = new Vector4(brightnessIndex, brightnessIndex, brightnessIndex, brightnessIndex);
            volume.profile.TryGet(out LiftGammaGain liftGammaGainOverride);
            brightnessSlider.value = brightnessIndex;
            PlayerPrefs.SetFloat("brightness", brightnessSlider.value);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.LogError("LiftGammaGain is null! Ensure the effect is enabled in the Volume Profile.");
        }
    }

    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt("quality", qualityIndex);
        PlayerPrefs.Save();

        QualitySettings.SetQualityLevel(qualityIndex);

        qualityDropdown.value = qualityIndex;
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}

