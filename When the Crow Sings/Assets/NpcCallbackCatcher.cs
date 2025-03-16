using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCallbackCatcher : MonoBehaviour
{
    public NpcInverseKinematicsHandler npcIKHandler;


    private void Start()
    {
        npcIKHandler.animator = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        npcIKHandler.MyOnAnimatorIK(layerIndex);
    }
}
