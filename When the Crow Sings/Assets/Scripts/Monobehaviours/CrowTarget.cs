using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowTarget : MonoBehaviour
{
    public float lifetime = 5.0f;
    public List<GameObject> subTargets = new List<GameObject>();

    public GameObject visualDebug;

    public void StartDisableCoroutine()
    {
        if (!isCoroutineRunning_DisableAfterTime) StartCoroutine(DisableAfterTime());
    }

    bool isCoroutineRunning_DisableAfterTime = true;
    public IEnumerator DisableAfterTime()
    {
        isCoroutineRunning_DisableAfterTime = true;
        GetComponent<NavMeshObstacle>().enabled = true;

        yield return new WaitForSeconds(lifetime);

        isCoroutineRunning_DisableAfterTime = false;
        Destroy(gameObject);
    }
}