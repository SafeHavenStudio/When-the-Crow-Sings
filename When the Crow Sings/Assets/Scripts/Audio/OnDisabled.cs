using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDisabled : MonoBehaviour
{
    public EventReference sound;

    private void OnDisable()
    {
        AudioManager.instance.PlayOneShot(sound);
        Debug.Log("Closed");
    }
}
