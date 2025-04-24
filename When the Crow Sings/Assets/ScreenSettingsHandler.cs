using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenSettingsHandler : MonoBehaviour, IService
{
    [SerializeField] Volume volume;
    LiftGammaGain liftGammaGain;

    List<Resolution> resolutions = new List<Resolution>();

    private void Awake()
    {
        RegisterSelfAsService();
    }

    private void Start()
    {
        TryGetLiftGammaGain();

        // Set resolutions (legacy code) Duplicated :(
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
    }

    void Update()
    {
        if (liftGammaGain == null) TryGetLiftGammaGain();
        else SetBrightness(GameSettings.GetModel().screenBrightness);

        //SetResolution(GameSettings.GetModel().screenResolutionIndex);
        SetQuality(GameSettings.GetModel().graphicsQualityIndex);

        //Screen.fullScreen = GameSettings.GetModel().fullScreenEnabled;
    }

    void SetBrightness(float _brightnessIndex)
    {
        //Debug.Log("Setting brightness to " + _brightnessIndex.ToString());
        liftGammaGain.gain.value = new Vector4(_brightnessIndex, _brightnessIndex, _brightnessIndex, _brightnessIndex);
    }

    public void SetResolution(int _resolutionIndex)
    {
        Resolution _resolution = resolutions[_resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, GameSettings.GetModel().fullScreenEnabled);
    }

    public void SetQuality(int _qualityIndex)
    {
        QualitySettings.SetQualityLevel(_qualityIndex);
    }

    void TryGetLiftGammaGain()
    {
        if (volume.profile.TryGet(out liftGammaGain))
        {
            Debug.Log("Successfully retrieved LiftGammaGain!");
        }
        else
        {
            Debug.LogError("Failed to find LiftGammaGain. Ensure the effect is added to your Volume Profile.");
        }
    }

    public void RegisterSelfAsService()
    {
        ServiceLocator.Register(this);
    }
}
