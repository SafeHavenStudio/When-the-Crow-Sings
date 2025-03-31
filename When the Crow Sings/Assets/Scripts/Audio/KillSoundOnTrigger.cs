using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSoundOnTrigger : MonoBehaviour
{
    private AreaMusic areaMusic;

    // Start is called before the first frame update
    void Start()
    {
        areaMusic = FindObjectOfType<AreaMusic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        areaMusic.ambienceInstance.setPaused(true);
        areaMusic.areaMusicInstance.setPaused(true);
        RuntimeManager.PlayOneShot(FMODEvents.instance.MenuClick);
    }

    private void OnTriggerExit(Collider other)
    {
        areaMusic.ambienceInstance.setPaused(false);
        areaMusic.areaMusicInstance.setPaused(false);
        Destroy(this);
    }
}
