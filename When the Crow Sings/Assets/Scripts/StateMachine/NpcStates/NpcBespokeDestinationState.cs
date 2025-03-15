using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBespokeDestinationState : NpcState
{
    public NpcBespokeDestinationState(NpcController component) : base(component) {}

    public override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Waypoint>() == s.currentWaypoint)
        {
            // Flip flag. May need more robust solution later.
            SaveDataAccess.saveData.boolFlags[s.GetComponent<DynamicEnable>().associatedDataKey] =
                !SaveDataAccess.saveData.boolFlags[s.GetComponent<DynamicEnable>().associatedDataKey];
            s.stateMachine.Enter("NpcIdleState");
        }
    }

    public override void StateEntered()
    {
        Debug.Log("Bespoken for.");
        s.animator.SetBool("animIsWalking", true);
        s.currentWaypoint = s.destinationForDialogueTriggeredMovement;
        s.navMeshAgent.destination = s.currentWaypoint.transform.position;
    }
    public override void StateExited()
    {
        s.animator.SetBool("animIsWalking", false);
    }
}