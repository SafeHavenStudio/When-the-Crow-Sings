using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType { 
        MASTER,
        MUSIC,
        AMBIENCE,
        SOUNDFX,
        TALKINGSOUND
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.instance.masterVolume;
                break;
            case VolumeType.MUSIC:
                volumeSlider.value = AudioManager.instance.musicVolume;
                break;
            case VolumeType.AMBIENCE:
                volumeSlider.value = AudioManager.instance.ambienceVolume;
                break;
            case VolumeType.SOUNDFX:
                volumeSlider.value = AudioManager.instance.soundFXVolume;
                break;
            case VolumeType.TALKINGSOUND:
                volumeSlider.value = AudioManager.instance.talkingSoundVolume;
                break;
            default:
                Debug.Log("Volume Type not supported: " + volumeType);
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.masterVolume = volumeSlider.value;
                break;
            case VolumeType.MUSIC:
                AudioManager.instance.musicVolume = volumeSlider.value;
                break;
            case VolumeType.AMBIENCE:
                AudioManager.instance.ambienceVolume = volumeSlider.value;
                break;
            case VolumeType.SOUNDFX:
                AudioManager.instance.soundFXVolume = volumeSlider.value;
                break;
            case VolumeType.TALKINGSOUND:
                AudioManager.instance.talkingSoundVolume = volumeSlider.value;
                break;
            default:
                Debug.Log("Volume Type not supported: " + volumeType);
                break;
        }
    }
}
