using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdseedManager : MonoBehaviour
{
    GameObject targetObject;

    [SerializeField]
    GameObject targetPrefab;

    public void OnBirseedLanded(SignalArguments args)
    {
        Debug.Log("Birdseed has landed!");

        if (targetObject == null)
        {
            targetObject = Instantiate(targetPrefab,transform);
            targetObject.transform.position = ((BirdseedController)args.objectArgs[0]).transform.position;
            Destroy(targetObject, 2f);
        }
        else
        {
            Debug.Log("Already a target in the scene!");
        }
    }
}
