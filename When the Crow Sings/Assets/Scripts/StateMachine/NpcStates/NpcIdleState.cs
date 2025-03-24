using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdleState : NpcState
{
    public NpcIdleState(NpcController component) : base(component) {}

    private IEnumerator exitStateAfterTime()
    {
        s.navMeshAgent.destination = s.transform.position;
        yield return new WaitForSeconds(s.timeToWaitBetweenWander);
        if (s.talkingState == NpcController.NpcState.TALKING)
        {
            s.StartCoroutine(exitStateAfterTime());
        }
        else
        {
            s.stateMachine.Enter("NpcPatrolState");
        }
        
    }

    public override void StateEntered()
    {
        s.animator.SetBool("animIsIdle", true);

        s.navMeshAgent.destination = s.transform.position;

        if (s.canWander)
        {
            if (s.WaypointsHolders.Count > 0) s.StartCoroutine(exitStateAfterTime());
            else throw new System.Exception(s.name + " is supposed to wander, but has no WaypointHolders!");
        }   
    }
    public override void StateExited()
    {
        s.animator.SetBool("animIsIdle", false);
    }
}