using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdseedManager : MonoBehaviour
{
    GameObject targetObject;

    [SerializeField]
    GameObject targetPrefab;

    [SerializeField]
    CrowManager crowManager;

    public void OnBirseedLanded(SignalArguments args)
    {
        Debug.Log("Birdseed has landed!");

        if (targetObject == null)
        {
            spawnTarget((BirdseedController)args.objectArgs[0]);

            crowManager.InformCrowsOfTarget(targetObject.GetComponent<CrowTarget>());
        }
        else
        {
            Debug.Log("Already a target in the scene!");
        }
    }

    void spawnTarget(BirdseedController birdseedController)
    {
        targetObject = Instantiate(targetPrefab, transform);
        targetObject.transform.position = birdseedController.transform.position;
    }
}
