using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayAudio : MonoBehaviour
{
    [field: SerializeField] public EventReference Sound { get; private set; }
    private Vector3 position;
    public bool playOnAwake;

    private void Start()
    {
        position = transform.position;

        if (playOnAwake)
        {
            PlayOneShot();
        }
    }


    public void PlayOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(Sound, position);  
    }
}
