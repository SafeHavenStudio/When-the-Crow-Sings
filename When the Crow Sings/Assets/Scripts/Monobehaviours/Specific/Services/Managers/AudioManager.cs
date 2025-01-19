using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;

    public static AudioManager instance { get; private set; }
    public AreaMusic areaMusic { get; private set; }

    [Range(0, 1)]
    public float masterVolume = 1;

    [Range(0, 1)]
    public float musicVolume = 1;

    [Range(0, 1)]
    public float ambienceVolume = 1;

    [Range(0, 1)]
    public float soundFXVolume = 1;

    [Range(0, 1)]
    public float talkingSoundVolume = 1;

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
