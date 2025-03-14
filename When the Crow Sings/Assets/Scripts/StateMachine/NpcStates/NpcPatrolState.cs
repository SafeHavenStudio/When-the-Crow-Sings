using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPatrolState : NpcState
{
    public NpcPatrolState(NpcController component) : base(component) {}

    public override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Waypoint>() == s.currentWaypoint)
        {
            //Debug.Log("Reached the next waypoint!");
            s.timeToWaitBetweenWander = other.GetComponent<Waypoint>().timeToWait;
            if (s.timeToWaitBetweenWander > 0)
                s.stateMachine.Enter("NpcIdleState");
            else setNextPoint();
        }
    }

    public override void StateEntered()
    {
        s.animator.SetBool("animIsWalking", true);
        setNextPoint();
    }
    public override void StateExited()
    {
        s.animator.SetBool("animIsWalking", false);
    }

    private void setNextPoint()
    {
        if (s.currentWaypoint != null)
        {
            s.currentWaypoint = s.currentWaypointHolder.GetNextWaypoint(s.currentWaypoint);
            s.navMeshAgent.destination = s.currentWaypoint.transform.position;
        }
        else
        {
            s.navMeshAgent.destination = new Vector3(0f, 0f, 0f);
        }
    }
}