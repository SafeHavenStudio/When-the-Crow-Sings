using FMOD;
using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class CrowTarget : MonoBehaviour
{
    public List<GameObject> subTargets = new List<GameObject>();
    List<GameObject> _remainingSubTargets;
    public GameObject visualDebug;

    public void StartActingAsObstacle(float lifetime) // Called by crows when they touch this.
    {
        GetComponent<NavMeshObstacle>().enabled = true;
        Destroy(gameObject, lifetime);
    }

    //List<int> indicesReturnedAlready = new List<int>();
    public GameObject GetRandomSubtarget()
    {
        //int randomIndex = GetRandomIndex();
        //indicesReturnedAlready.Add(randomIndex);
        //return subTargets[randomIndex];
        if (_remainingSubTargets.Count <= 0)
        {
            ResetRandomness();
        }

        int _index = UnityEngine.Random.Range(0, _remainingSubTargets.Count - 1);
        GameObject randomSubtarget = _remainingSubTargets[_index];
        _remainingSubTargets.Remove(randomSubtarget);

        return randomSubtarget;
    }

    public void ResetRandomness()
    {
        _remainingSubTargets = new List<GameObject>(subTargets);
    }

    //int GetRandomIndex()
    //{
    //    int _index;
    //    _index = UnityEngine.Random.Range(0, subTargets.Count - 1);
    //    if (indicesReturnedAlready.Contains(_index))
    //    {
    //        GetRandomIndex();
    //    }
    //    return _index;
    //}
}