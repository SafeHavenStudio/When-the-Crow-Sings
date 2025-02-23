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
        Debug.Log("AAAAAH");
        s.StartCoroutine(Wait());
    }
    public override void Update(float deltaTime)
    {
        s.ApplyGravity(deltaTime);

        Vector3 directionToInteractable = (s.transform.position - s.mostRecentInteractable.transform.position).normalized*3;

        s.characterController.Move(directionToInteractable * deltaTime);
        s.transform.Rotate(new Vector3(0, -1000, 0) * deltaTime);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(s.timeToFear);
        s.stateMachine.Enter("PlayerMovementState");
    }
}
