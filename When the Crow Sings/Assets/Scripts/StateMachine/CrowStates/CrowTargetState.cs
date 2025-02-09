using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowTargetState : StateMachineState
{
    BirdBrain s;
    public CrowTargetState(BirdBrain birdBrain)
    {
        s = birdBrain;
    }

    public override void FixedUpdate()
    {
        //s.targetPosition = (s.destination - s.transform.position)*.01f;
        s.FlyNavigate_FixedUpdate();
    }
    public override void StateEntered()
    {
        if (s.targetIsTargetNotSpawn) setupSeedDestination();
        else s.destination = s.restPoint.transform.position;
        s.crowAnimator.SetBool("isFlying", true);
        s.crowAnimator.SetBool("isIdle", false);
        s.crowAnimator.SetBool("isPecking", false);
    }

    void setupSeedDestination()
    {
        //s.destination = s.crowHolder.crowTarget.transform.position;
        CalculateTimeToTarget();

        Debug.Log(s.name + " is too far away! Teleporting!");
        float distanceToTeleport = s.flyingSpeed * CalculateTimeToTarget() * 60;
        Debug.Log("Distance to Teleport: " + distanceToTeleport.ToString());
        s.controller.enabled = false;
        s.transform.position = Vector3.MoveTowards(s.transform.position, s.destination, -distanceToTeleport);
        s.controller.enabled = true;

        if (Mathf.Sign(CalculateTimeToTarget()) == -1)
        {
            
        }


    }

    private float CalculateTimeToTarget() // TODO: Move this to be a delay before it takes off.
    {
        float distanceToSeed = (s.transform.position - s.destination).magnitude;
        float timeItWouldTakeToReachSeed = -((distanceToSeed / s.flyingSpeed) / 60) + s.timeAllowedToReachBirdseed; // TODO: make sure this is right.
        //Debug.Log("Time: " + timeItWouldTakeToReachSeed.ToString() + " and WaitTime should be " + s.timeAllowedToReachBirdseed.ToString());

        return timeItWouldTakeToReachSeed;
    }
}
