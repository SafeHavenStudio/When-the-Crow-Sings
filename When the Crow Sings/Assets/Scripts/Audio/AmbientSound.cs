using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AmbientSound : MonoBehaviour
{
    [field: SerializeField] public EventReference Sound { get; private set; }

    private float minInterval = 5f;  //Minimum time between sounds
    private float maxInterval = 15f; //Maximum time between sounds
    public bool playOnAwake = true;
    public bool threeDSound;
    //private Vector3 transform;

    private void Start()
    {
        if(playOnAwake || !threeDSound)
        StartCoroutine(PlayAmbientSound());

        if(playOnAwake || threeDSound)
        StartCoroutine(PlayAmbientSound3D());
    }

    private IEnumerator PlayAmbientSound()
    {
        while (true)
        {
            float randomDelay = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomDelay);

            FMODUnity.RuntimeManager.PlayOneShot(Sound);
        }
    }

    private IEnumerator PlayAmbientSound3D()
    {
        while (true)
        {
            float randomDelay = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomDelay);

            FMODUnity.RuntimeManager.PlayOneShot(Sound, this.transform.position);
        }
    }
}
