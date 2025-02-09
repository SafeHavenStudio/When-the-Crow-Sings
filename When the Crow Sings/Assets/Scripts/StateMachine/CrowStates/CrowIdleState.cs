using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowIdleState : StateMachineState
{
    BirdBrain s;

    public CrowIdleState(BirdBrain birdBrain)
    {
        s = birdBrain;
    }

    public override void FixedUpdate()
    {
        s.ApplyGravityWhileStill();
    }

    public override void StateEntered()
    {
        s.controller.enabled = false;
        s.transform.SetPositionAndRotation(s.restPoint.transform.position + new Vector3(0,1,0),s.restPoint.transform.rotation);
        s.controller.enabled = true;
        s.crowAnimator.SetBool("isFlying", false);
        s.crowAnimator.SetBool("isIdle", true);
        s.crowAnimator.SetBool("isPecking", false);
    }
}
