using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    public EnemyPatrolState(EnemyController component) : base(component) {}

    //private float enemySpeed = 0;
    //private Vector3 lastPosition;
    //public override void FixedUpdate()
    //{
    //    if (lastPosition == null) lastPosition = s.transform.position;

    //    enemySpeed = (s.transform.position - lastPosition).magnitude;
    //    lastPosition = s.transform.position;
    //}

    public override void OnTriggerEnter(Collider other)
    {
        //Debug.Log("I SEE YOU THERE IS NO LIFE IN THE VOID DIE NOW");
        //s.stateMachine.Enter("EnemyChaseState");
        if (other.GetComponent<Waypoint>() == s.currentWaypoint)
        {
            //Debug.Log("Reached the next waypoint!");
            s.timeToWaitBetweenWander = other.GetComponent<Waypoint>().timeToWait;
            if (s.timeToWaitBetweenWander > 0)
                s.stateMachine.Enter("EnemyIdleState");
            else setNextPoint();
        }
    }

    public override void FixedUpdate()
    {
        if (s.doesSeePlayer)
        {
            s.EnterChaseStateSafe();
        }
    }


    public override void StateEntered()
    {
        s.navMeshAgent.speed = s.patrolSpeed;
        s.enemyAnimator.SetBool("animIsPatrolWalking", true);
        setNextPoint();
    }
    public override void StateExited()
    {
        s.enemyAnimator.SetBool("animIsPatrolWalking", false);
    }

    private void setNextPoint()
    {
        //Debug.Log("Setting next point!");
        
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