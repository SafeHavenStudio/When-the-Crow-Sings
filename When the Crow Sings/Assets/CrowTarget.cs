using ScriptableObjects;
using System.Collections;
using UnityEngine;

public class CrowTarget : MonoBehaviour
{
    public float SecondsToAttractCrows = 5.0f;
    [HideInInspector]
    public bool isActiveTarget = false;
    

    public GameObject visualDebug;

    public GameSignal enabledSignal;
    public GameSignal disabledSignal;

    public void SetActiveTarget()
    {
        isActiveTarget = true;
        enabledSignal.Emit();
        visualDebug.SetActive(true);
        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(SecondsToAttractCrows);
        isActiveTarget = false;

        Destroy(ServiceLocator.Get<GameManager>().activeBirdseed.gameObject);
        ServiceLocator.Get<GameManager>().activeBirdseed = null;
        disabledSignal.Emit();
        
        visualDebug.SetActive(false);
    }
}