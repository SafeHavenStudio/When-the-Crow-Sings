using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayAudio : MonoBehaviour
{
    [field: SerializeField] public EventReference Sound { get; private set; }
    public static FMODEvents instance { get; private set; }
    [HideInInspector] public EventInstance areaMusicInstance;
    private Vector3 position;
    public bool playOnAwake;

    private void Start()
    {
        position = transform.position;

        if (playOnAwake)
        {
            PlayOneShot();
        }

        if (!Sound.IsNull) areaMusicInstance = AudioManager.instance.CreateEventInstance(FMODEvents.instance.AreaMusic);
    }


    public void PlayOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Sound, position);  
    }

    public void OnDestroy()
    {
        if (areaMusicInstance.isValid())
        {
            areaMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            areaMusicInstance.release();
        }
    }
}
