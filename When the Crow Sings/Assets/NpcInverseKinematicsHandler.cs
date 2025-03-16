using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Animator))]
public class NpcInverseKinematicsHandler : MonoBehaviour
{
    public bool playerInSightCone = false;

    public Animator animator;

    GameObject playerHeadPosition;

    private float lookAtWeight = 0f;

    public void TriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) playerInSightCone = true;
    }

    public void TriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) playerInSightCone = false;
    }

    float lerpSpeed = 2f;
    private void Update()
    {
        lookAtWeight = Mathf.Lerp(lookAtWeight, playerInSightCone ? 1.0f : 0.0f, Time.deltaTime * lerpSpeed);
    }
    public void MyOnAnimatorIK(int layerIndex)
    {
        Debug.Log("Animator is being IK'ed mate, lovely.");

        if (playerHeadPosition == null) playerHeadPosition = GameObject.FindWithTag("PlayerHeadPosition");

        animator.SetLookAtWeight(lookAtWeight);
        animator.SetLookAtPosition(playerHeadPosition.transform.position);
    }
}
