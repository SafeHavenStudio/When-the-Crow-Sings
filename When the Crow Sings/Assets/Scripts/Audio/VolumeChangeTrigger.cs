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
    [Tooltip("Volume -10 to 10")]
    [SerializeField] private float parametervalue;

    public AreaMusic areaMusic;

    private void Start()
    {
        StartCoroutine(WaitForAreaMusicCheck());
    }


    private IEnumerator WaitForAreaMusicCheck()
    {
        yield return new WaitForSeconds(1f);

        areaMusic = FindObjectOfType<AreaMusic>();
        if (areaMusic == null)
        {
            Debug.Log("Could not find area music. Check additive scenes");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            areaMusic.SetParameter(parameterName, parametervalue);
        }
    }
}
