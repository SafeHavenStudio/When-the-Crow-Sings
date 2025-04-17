using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using FMODUnity;
using FMOD.Studio;

public class VolumeChangeTrigger : MonoBehaviour
{
    [Tooltip("Name of the sound")]
    [SerializeField] private string parameterName;
    [Tooltip("Volume 0 to 1")]
    [SerializeField] private float parametervalue;

    public AreaMusic areaMusic;

    private void Start()
    {
        StartCoroutine(WaitForAreaMusicCheck());
    }


    private IEnumerator WaitForAreaMusicCheck()
    {
        yield return new WaitForSeconds(1f);

        if (areaMusic == null)
        {
            areaMusic = FindObjectOfType<AreaMusic>();
            Debug.Log("Volume change trigger Grabbed area music");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            areaMusic.SetParameter(parameterName, parametervalue);
            Debug.Log("Changing volume" + parameterName + " to volume " + parametervalue);
        }
    }
}
