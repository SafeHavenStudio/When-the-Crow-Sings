using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayAudio : MonoBehaviour
{
    //Use this script to attach a single audio instance to an event

    //[field: SerializeField] public EventReference Sound { get; private set; }
    public bool playOnAwake;

    private void Start()
    {
        if (playOnAwake)
        {
            PlayOneShot();
        }
    }

    public void PlayOneShot()
    {
        //FMODUnity.RuntimeManager.PlayOneShot(Sound);
    }
}
