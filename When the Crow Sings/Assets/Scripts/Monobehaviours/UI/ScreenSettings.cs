using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ScreenSettings : MonoBehaviour
{
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
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

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && 
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 0f);
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
        qualityIndex = PlayerPrefs.GetInt("quality", 2);
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
        PlayerPrefs.Save();
    }

    public void setFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
