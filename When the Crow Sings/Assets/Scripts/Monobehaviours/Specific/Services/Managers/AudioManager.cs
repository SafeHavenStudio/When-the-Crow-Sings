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

    public bool penClickSound = true;

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
    }



    private void Update()
    {
        masterBus.setVolume(GameSettings.GetModel().masterVolume);
        musicBus.setVolume(GameSettings.GetModel().musicVolume);
        ambienceBus.setVolume(GameSettings.GetModel().ambienceVolume);
        soundFXBus.setVolume(GameSettings.GetModel().soundFxVolume);
        talkingSoundBus.setVolume(GameSettings.GetModel().voicesVolume);

        penClickSound = GameSettings.GetModel().penClick;
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
