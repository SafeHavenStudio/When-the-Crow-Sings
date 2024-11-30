using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AreaMusic : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference Music { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference Ambience { get; private set; }

    private EventInstance areaMusicInstance;
    private EventInstance ambienceInstance;

    private void Start()
    {
        if (FMODEvents.instance == null)
        {
            Debug.LogError("FMODEvents is not initialized. Ensure FMODEvents is present in the scene.");
            return;
        }

        //Dynamically assign to FMODEvents
        FMODEvents.instance.SetDynamicAssignment(Music, Ambience);

        areaMusicInstance = AudioManager.instance.CreateEventInstance(FMODEvents.instance.AreaMusic);
        ambienceInstance = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Ambience);

        PlayMusic();
        PlayAmbience();
    }

    private void PlayMusic()
    {
        if (areaMusicInstance.isValid())
        {
            areaMusicInstance.start();
            Debug.Log("Playing Area Music.");
        }
        else
        {
            Debug.LogError("Area music instance is invalid.");
        }
    }

    private void PlayAmbience()
    {
        if (ambienceInstance.isValid())
        {
            ambienceInstance.start();
            Debug.Log("Playing Ambience.");
        }
        else
        {
            Debug.LogError("Ambience instance is invalid.");
        }
    }

    private void OnDestroy()
    {
        if (areaMusicInstance.isValid())
        {
            areaMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            areaMusicInstance.release();
        }

        if (ambienceInstance.isValid())
        {
            ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambienceInstance.release();
        }
    }
}

