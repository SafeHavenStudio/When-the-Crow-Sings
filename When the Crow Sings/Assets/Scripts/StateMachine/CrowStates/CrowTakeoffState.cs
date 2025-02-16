using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowTakeoffState : StateMachineState
{
    BirdBrain s;
    public CrowTakeoffState(BirdBrain birdBrain)
    {
        s = birdBrain;
    }
    
    public override void FixedUpdate()
    {
        s.controller.Move((s.transform.forward)*.02f);
    }

    public override void StateEntered()
    {
        s.crowAnimator.SetBool("isFlying", true);
        s.crowAnimator.SetBool("isIdle", false);
        s.crowAnimator.SetBool("isPecking", false);

        s.StartCoroutine(WaitThenEnterTargetState());
    }

    IEnumerator WaitThenEnterTargetState()
    {
        yield return new WaitForSeconds(.5f);
        s.stateMachine.Enter("CrowTargetState");
    }
}
