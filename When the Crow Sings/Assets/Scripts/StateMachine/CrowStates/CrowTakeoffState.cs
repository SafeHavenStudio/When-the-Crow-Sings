using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
// UnityEditorInternal;
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

        // TODO: Perhaps check if the birdseed is close enough to instantly enter the 'peck' state even if it's already touching it and can't 'touch' it again?

        if (s.stateMachine.previousState == s.stateMachine.states["CrowTargetState"])
        {
            s.stateMachine.Enter("CrowTargetState");
        }
        else
        {
            s.crowAnimator.SetBool("isFlying", true);
            s.crowAnimator.SetBool("isIdle", false);
            s.crowAnimator.SetBool("isPecking", false);

            s.crowAnimator.SetBool("isInTransitionState", true);

            s.StartCoroutine(WaitThenEnterTargetState());
        }        
    }

    IEnumerator WaitThenEnterTargetState()
    {
        yield return new WaitForSeconds(.5f);


        s.crowAnimator.SetBool("isInTransitionState", false);
        s.stateMachine.Enter("CrowTargetState");
    }
}
