using ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
        isDisableAfterTimeRunning = false;
        isActiveTarget = true;
        enabledSignal.Emit();
        //visualDebug.SetActive(true);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<BirdBrain>() != null)
    //    {
    //        if (!isDisableAfterTimeRunning) StartCoroutine(DisableAfterTime());
    //    }
    //}

    public bool isDisableAfterTimeRunning = true;

    public void StartDisable()
    {
        if (!isDisableAfterTimeRunning) StartCoroutine(DisableAfterTime());
    }
    public IEnumerator DisableAfterTime()
    {
        isDisableAfterTimeRunning = true;
        GetComponent<NavMeshObstacle>().enabled = true;
        yield return new WaitForSeconds(SecondsToAttractCrows);
        
        
        DisableTarget();
    }

    public void DisableTarget()
    {
        isActiveTarget = false;

        //if (ServiceLocator.Get<GameManager>().activeBirdseed != null)
        //    Destroy(ServiceLocator.Get<GameManager>().activeBirdseed.gameObject);
        //ServiceLocator.Get<GameManager>().activeBirdseed = null;
        disabledSignal.Emit();

        visualDebug.SetActive(false);
        isDisableAfterTimeRunning = false;
        GetComponent<NavMeshObstacle>().enabled = false;
    }
}