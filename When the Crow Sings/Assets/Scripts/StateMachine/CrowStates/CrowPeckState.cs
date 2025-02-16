using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowPeckState : StateMachineState
{
    BirdBrain s;
    public CrowPeckState(BirdBrain birdBrain)
    {
        s = birdBrain;
    }

    public override void FixedUpdate()
    {
        s.ApplyGravityWhileStill();
    }

    public override void StateEntered()
    {
        s.transform.localRotation = Quaternion.Euler(0f, s.transform.localRotation.eulerAngles.y, s.transform.localRotation.eulerAngles.z);
        s.StartCoroutine(ExitStateAfterSeconds());

        s.crowAnimator.SetBool("isFlying", false);
        s.crowAnimator.SetBool("isIdle", false);
        s.crowAnimator.SetBool("isPecking", true);
    }

    IEnumerator ExitStateAfterSeconds() // TODO: Figure out a way to make all birds end simultaneously to avoid potential issues.
    {
        yield return new WaitForSeconds(s.secondsToPeck);

        s.finishedEating.Invoke();

        //s.StartFlyingTowardRestPoint();
    }
}
