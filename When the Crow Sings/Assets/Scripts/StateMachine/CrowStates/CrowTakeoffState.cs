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
        //s.FlyNavigate_FixedUpdate();
        s.controller.Move(Vector3.up * 0.25f);
    }

    public override void StateEntered()
    {
        //float range = 20f;
        //s.destination = new Vector3(Random.Range(-range,range), Random.Range(range, range), Random.Range(-range, range));
        s.StartCoroutine(WaitThenEnterTargetState());
        s.crowAnimator.SetBool("isFlying", true);
        s.crowAnimator.SetBool("isIdle", false);
        s.crowAnimator.SetBool("isPecking", false);
    }

    IEnumerator WaitThenEnterTargetState()
    {
        yield return new WaitForSeconds(.25f);
        s.stateMachine.Enter("CrowTargetState");
    }
}
