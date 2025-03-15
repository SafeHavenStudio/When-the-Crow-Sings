using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcReturnToOriginalPositionState : NpcState
{
    public NpcReturnToOriginalPositionState(NpcController component) : base(component) {}

    public override void Update(float deltaTime)
    {
        if ((s.transform.position - s.originalPosition).magnitude <= 1f)
        {
            s.stateMachine.Enter("NpcIdleState");
        }
    }

    public override void StateEntered()
    {
        s.animator.SetBool("animIsWalking", true);
        s.currentWaypoint = null; // May not be necessary?
        s.navMeshAgent.destination = s.originalPosition;
    }
    public override void StateExited()
    {
        Debug.Log("Made it back safely.");
        s.animator.SetBool("animIsWalking", false);
    }
}