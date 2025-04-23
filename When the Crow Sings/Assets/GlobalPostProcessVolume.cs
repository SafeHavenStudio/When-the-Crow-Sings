using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalPostProcessVolume : MonoBehaviour
{
    [SerializeField] Volume volume;
    LiftGammaGain liftGammaGain;

    private void Start()
    {
        TryGetLiftGammaGain();
    }

    // Update is called once per frame
    void Update()
    {
        if (liftGammaGain == null)
        {
            TryGetLiftGammaGain();
        }
        else
        {
            SetBrightness(GameSettings.GetModel().screenBrightness);
        }
    }

    public void SetBrightness(float _brightnessIndex)
    {
        Debug.Log("Setting brightness to " + _brightnessIndex.ToString());
        liftGammaGain.gain.value = new Vector4(_brightnessIndex, _brightnessIndex, _brightnessIndex, _brightnessIndex);
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
}
