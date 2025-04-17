using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEngine.Rendering.DebugUI;

public class NpcInverseKinematicsHandler : MonoBehaviour
{
    [HideInInspector]
    public bool playerInSightCone = false;

    GameObject playerHeadPosition;

    public RigBuilder npcRigBuilder;
    public Transform npcLookAtPointTransform;

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
        if (npcRigBuilder == null) return;

        lookAtWeight = Mathf.Lerp(lookAtWeight, playerInSightCone ? 1.0f : 0.0f, Time.deltaTime * lerpSpeed);

        if (playerHeadPosition == null) playerHeadPosition = GameObject.FindWithTag("PlayerHeadPosition");

        npcRigBuilder.layers[0].rig.weight = lookAtWeight;
        npcLookAtPointTransform.position = playerHeadPosition.transform.position;
        //animator.SetLookAtWeight(lookAtWeight);
        //animator.SetLookAtPosition(playerHeadPosition.transform.position);
    }
}
