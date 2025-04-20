using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PlayerFrozenState : StateMachineState
{
    PlayerController s;

    public PlayerFrozenState(PlayerController component)
    {
        s = component;
    }

    public override void StateEntered()
    {
        InputManager.playerInputActions.Player.Disable();
        InputManager.playerInputActions.UI.Enable();
    }
    public override void StateExited()
    {
        InputManager.playerInputActions.Player.Enable();
        InputManager.playerInputActions.UI.Disable();
        s.playerAnimator.SetBool("isTalking", false);
    }

    public override void Update(float deltaTime)
    {
        s.ApplyGravity(deltaTime);
        s.characterController.Move(new Vector3(0f,s.gravityVelocity,0f));
        s.playerAnimator.SetBool("isTalking", DialogueManager.whoIsTalking == DialoguePortraits.WhoIsTalking.Chance);
    }
}
