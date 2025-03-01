using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFearState : StateMachineState
{
    PlayerController s;

    public PlayerFearState(PlayerController component)
    {
        s = component;
    }

    public override void StateEntered()
    {
        s.playerAnimator.SetBool("animIsFear", true);
        s.StartCoroutine(Wait());
    }
    public override void StateExited()
    {
        s.playerAnimator.SetBool("animIsFear", false);
    }
    public override void Update(float deltaTime)
    {
        s.ApplyGravity(deltaTime);

        Vector3 directionToInteractable = (s.transform.position - s.mostRecentInteractable.transform.position);
        directionToInteractable.y = 0f;
        directionToInteractable = directionToInteractable.normalized * 3;

        s.characterController.Move(directionToInteractable * deltaTime);
        //s.transform.Rotate(new Vector3(0, -1000, 0) * deltaTime);
        s.transform.rotation = Quaternion.LookRotation(-directionToInteractable);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(s.timeToFear);
        if (s.isInteracting) s.stateMachine.Enter("PlayerFrozenState");
        else s.stateMachine.Enter("PlayerMovementState");
    }
}
