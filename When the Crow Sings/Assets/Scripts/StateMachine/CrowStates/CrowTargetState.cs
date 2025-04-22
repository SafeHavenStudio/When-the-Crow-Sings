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
        s.FlyNavigate_FixedUpdate();
    }
    public override void StateEntered()
    {
        if (s.target != s.restPoint.transform) setupSeedDestination();
        s.crowAnimator.SetBool("isFlying", true);
        s.crowAnimator.SetBool("isIdle", false);
        s.crowAnimator.SetBool("isPecking", false);

        s.controller.excludeLayers = s.whenFlyingMask;
    }
    public override void StateExited()
    {
        s.controller.excludeLayers = new LayerMask();
    }

    void setupSeedDestination()
    {
        s.CalculateTimeToTarget();

        //Debug.Log(s.name + " is too far away! Teleporting!");
        float distanceToTeleport = s.flyingSpeed * s.CalculateTimeToTarget() * 60;
        //Debug.Log("Distance to Teleport: " + distanceToTeleport.ToString());

        if (Mathf.Sign(s.CalculateTimeToTarget()) == -1)
        {
            s.controller.enabled = false;
            s.transform.position = Vector3.MoveTowards(s.transform.position, s.target.position, -distanceToTeleport);
            s.controller.enabled = true;
        }
    }
}
