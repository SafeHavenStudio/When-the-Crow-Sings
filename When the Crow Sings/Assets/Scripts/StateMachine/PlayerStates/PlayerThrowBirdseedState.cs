using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerThrowBirdseedState : StateMachineState
{
    PlayerController s;
    bool canThrow = false;
    bool hasThrownAlready = false;

    public PlayerThrowBirdseedState(PlayerController component)
    {
        s = component;
    }

    public override void StateEntered()
    {
        hasThrownAlready = false;
        s.StartCoroutine(WaitBeforeThrowing());
        InputManager.playerInputActions.Player.Fire.canceled += OnFire;

        s.throwTarget.SetActive(true);
        s.trajectoryLine.SetActive(true);

        InputManager.playerInputActions.Player.Move.performed += OnMoveButBirdseedNow;
        InputManager.playerInputActions.Player.Move.canceled += OnMoveButBirdseedNow;

    }
    public override void StateExited()
    {
        if (!hasThrownAlready) // This is to prevent things from breaking if the player leaves the state prematurely.
        {
            InputManager.playerInputActions.Player.Fire.canceled -= OnFire;
            ThrowBirdseed();
        }

        s.playerAnimator.SetLayerWeight(1, 0f);

        InputManager.playerInputActions.Player.Move.performed -= OnMoveButBirdseedNow;
        InputManager.playerInputActions.Player.Move.canceled -= OnMoveButBirdseedNow;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        s.StartCoroutine(ThrowBirdseedOnceAble());
    }

    public override void Update(float deltaTime)
    {
        //Quaternion toRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
        //s.transform.rotation = Quaternion.RotateTowards(s.transform.rotation, toRotation, 1000 * deltaTime);

        Vector3 rotationToTarget = - s.transform.position + s.throwTarget.transform.position;
        rotationToTarget.y = 0f;
        Quaternion quaternionRotation = Quaternion.LookRotation(rotationToTarget, Vector3.up);
        s.transform.rotation = quaternionRotation;
    }

    private IEnumerator WaitBeforeThrowing()
    {
        canThrow = false;
        float weight = 0.0f;
        float transitionSpeed = 7.0f;
        while (weight < 1.0f)
        {
            weight += transitionSpeed * Time.deltaTime;
            weight = Mathf.Clamp01(weight);
            s.playerAnimator.SetLayerWeight(1, weight);
            yield return null;
        }
        canThrow = true;
    }

    private IEnumerator ThrowBirdseedOnceAble()
    {
        InputManager.playerInputActions.Player.Fire.canceled -= OnFire;

        while (!canThrow) yield return null;

        ThrowBirdseed();
       
    }
    private void ThrowBirdseed()
    {
        s.ThrowBirdseed();

        SaveDataAccess.SetFlag("HasThrownBirdseed", true);

        s.playerAnimator.SetTrigger("animThrow");

        s.throwTarget.SetActive(false);
        s.trajectoryLine.SetActive(false);
        hasThrownAlready = true;
    }

    private void OnMoveButBirdseedNow(InputAction.CallbackContext context)
    {
        s.throwTarget.GetComponent<ThrowTarget>().controllerInput = context.ReadValue<Vector2>(); // Normal movement
    }
}
