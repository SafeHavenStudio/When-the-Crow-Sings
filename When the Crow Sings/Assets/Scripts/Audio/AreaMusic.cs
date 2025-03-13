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

    public List<StudioEventEmitter> emitters;
    StudioEventEmitter pickedSong;

    [HideInInspector] public EventInstance areaMusicInstance;
    [HideInInspector] public EventInstance ambienceInstance;

    private void Start()
    {
        if (FMODEvents.instance == null)
        {
            Debug.LogError("FMODEvents is not initialized. Ensure FMODEvents is present in the scene.");
            return;
        }

        //Dynamically assign to FMODEvents
        FMODEvents.instance.SetDynamicAssignment(Music, Ambience, emitters);

        if (!Music.IsNull) areaMusicInstance = AudioManager.instance.CreateEventInstance(FMODEvents.instance.AreaMusic);
        if (!Ambience.IsNull) ambienceInstance = AudioManager.instance.CreateEventInstance(FMODEvents.instance.Ambience);


        PlayMusic();
        PlayAmbience();
        PlayRandomSong();
    }

    public void SetParameter(string parameterName, float parameterValue)
    {
        ambienceInstance.setParameterByName(parameterName, parameterValue);
    }

    private void PlayMusic()
    {
        if (!Music.IsNull)
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
    }

    private void PlayAmbience()
    {
        if (!Ambience.IsNull)
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
        
    }

    private void PlayRandomSong()
    {
        if (emitters.Count > 0)
        {
            int i = Random.Range(0, emitters.Count);

            pickedSong = emitters[i];

            if (pickedSong != null)
            {
                pickedSong.Play();
                Debug.Log("Playing" + pickedSong);
            }
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

        if (emitters.Count > 0)
        {
            pickedSong.Stop();
        }
    }
}

