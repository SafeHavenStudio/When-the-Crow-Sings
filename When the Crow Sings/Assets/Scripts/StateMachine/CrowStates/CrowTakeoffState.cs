using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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


        //s.controller.Move(Vector3.up * 0.25f);
        s.controller.Move((s.transform.forward)*.02f);
    }

    public override void StateEntered()
    {
        //float range = 20f;
        //s.destination = new Vector3(Random.Range(-range,range), Random.Range(range, range), Random.Range(-range, range));

        if (s.stateMachine.previousState == s.stateMachine.states["CrowTargetState"])
        {
            s.stateMachine.Enter("CrowTargetState");
        }
        else
        {
            s.crowAnimator.SetBool("isFlying", true);
            s.crowAnimator.SetBool("isIdle", false);
            s.crowAnimator.SetBool("isPecking", false);

            s.StartCoroutine(WaitThenEnterTargetState());
        }        
    }

    IEnumerator WaitThenEnterTargetState()
    {
        yield return new WaitForSeconds(.5f);
        s.stateMachine.Enter("CrowTargetState");
    }
}
