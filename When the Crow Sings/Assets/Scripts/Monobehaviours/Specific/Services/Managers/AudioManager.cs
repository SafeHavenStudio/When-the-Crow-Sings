using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;

    public static AudioManager instance { get; private set; }
    public AreaMusic areaMusic { get; private set; }

    [Range(0, 1)]
    public float masterVolume = 1f;

    [Range(0, 1)]
    public float musicVolume = 0.5f;

    [Range(0, 1)]
    public float ambienceVolume = 0.5f;

    [Range(0, 1)]
    public float soundFXVolume = 0.5f;

    [Range(0, 1)]
    public float talkingSoundVolume = 0.5f;

    public bool penClickSound = true;
    public Toggle penClickToggle;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus soundFXBus;
    private Bus talkingSoundBus;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one audio manager in the scene");
        }
        instance = this;

        eventInstances = new List<EventInstance>();

        areaMusic = FindObjectOfType<AreaMusic>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        soundFXBus = RuntimeManager.GetBus("bus:/SoundFX");
        talkingSoundBus = RuntimeManager.GetBus("bus:/TalkingSound");

        masterVolume = PlayerPrefs.GetFloat("masterVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        ambienceVolume = PlayerPrefs.GetFloat("ambienceVolume", 0.5f);
        soundFXVolume = PlayerPrefs.GetFloat("soundFXVolume", 0.5f);
        talkingSoundVolume = PlayerPrefs.GetFloat("talkingSoundVolume", 0.5f);

        penClickSound = PlayerPrefs.GetInt("PenClick", 1) != 0;

        if (penClickToggle != null)
        {
            penClickToggle.isOn = penClickSound;
            penClickToggle.onValueChanged.AddListener(delegate { toggleSwitch(); });
        }
    }



    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        soundFXBus.setVolume(soundFXVolume);
        talkingSoundBus.setVolume(talkingSoundVolume);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayOneShot(EventReference sound) // hopefully this overload removes the spatialness?
    {
        RuntimeManager.PlayOneShot(sound);
    }

    public bool IsSoundPlaying(EventReference sound)
    {
        return false;
    }

    public void toggleSwitch()
    {
        if (penClickToggle == null) return;

        penClickSound = penClickToggle.isOn;
        PlayerPrefs.SetInt("PenClick", penClickSound ? 1 : 0);
        PlayerPrefs.Save();
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        if (eventReference.IsNull)
        {
            Debug.LogWarning("Attempted to create an EventInstance with a null EventReference.");
            return default;
        }

        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
