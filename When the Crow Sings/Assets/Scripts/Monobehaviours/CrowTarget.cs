using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class CrowTarget : MonoBehaviour
{
    public List<GameObject> subTargets = new List<GameObject>();

    public GameObject visualDebug;

    public void StartActingAsObstacle(float lifetime) // Called by crows when they touch this.
    {
        GetComponent<NavMeshObstacle>().enabled = true;
        Destroy(gameObject, lifetime);
    }

    public GameObject GetRandomSubtarget()
    {
        int randomIndex = UnityEngine.Random.Range(0, subTargets.Count - 1);
        return subTargets[randomIndex];
    }
}