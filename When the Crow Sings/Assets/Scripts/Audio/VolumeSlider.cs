using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
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

        //Load saved volume settings
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
        }

        //Listen for slider changes
        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OnSliderValueChanged(float value)
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.masterVolume = value;
                PlayerPrefs.SetFloat("masterVolume", value);
                break;
            case VolumeType.MUSIC:
                AudioManager.instance.musicVolume = value;
                PlayerPrefs.SetFloat("musicVolume", value);
                break;
            case VolumeType.AMBIENCE:
                AudioManager.instance.ambienceVolume = value;
                PlayerPrefs.SetFloat("ambienceVolume", value);
                break;
            case VolumeType.SOUNDFX:
                AudioManager.instance.soundFXVolume = value;
                PlayerPrefs.SetFloat("soundFXVolume", value);
                break;
            case VolumeType.TALKINGSOUND:
                AudioManager.instance.talkingSoundVolume = value;
                PlayerPrefs.SetFloat("talkingSoundVolume", value);
                break;
        }

        PlayerPrefs.Save();
    }
}

