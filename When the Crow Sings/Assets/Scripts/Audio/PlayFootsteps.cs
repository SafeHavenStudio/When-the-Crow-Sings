using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayFootsteps : MonoBehaviour
{
    [field: SerializeField] public EventReference ConcreteFootsteps { get; private set; }
    [field: SerializeField] public EventReference GrassFootsteps { get; private set; }
    [field: SerializeField] public EventReference IndoorFootsteps { get; private set; }
    [field: SerializeField] public EventReference DefaultFootsteps { get; private set; }
    [field: SerializeField] public EventReference DirtFootsteps { get; private set; }
    [field: SerializeField] public EventReference GravelFootsteps { get; private set; }
    [field: SerializeField] public EventReference LeafFootsteps { get; private set; }


    public void PlayOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(DefaultFootsteps, this.transform.position);
        //Debug.Log("Step");

    }
}
