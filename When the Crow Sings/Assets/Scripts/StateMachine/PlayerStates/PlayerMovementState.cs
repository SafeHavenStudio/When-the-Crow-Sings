using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : StateMachineState
{
    PlayerController s;

    public PlayerMovementState(PlayerController component)
    {
        s = component;
    }

    public override void StateEntered()
    {
        //Debug.Log("Entering MovementState");

        // Subscribe to the Move event in the input system
        InputManager.playerInputActions.Player.Move.performed += OnMove;
        InputManager.playerInputActions.Player.Move.canceled += OnMove;

        InputManager.playerInputActions.Player.Action.performed += OnAction;
        InputManager.playerInputActions.Player.Action.canceled += OnAction;

        InputManager.playerInputActions.Player.Interact.performed += OnInteract;
        InputManager.playerInputActions.Player.Interact.canceled += OnInteract;

        InputManager.playerInputActions.Player.Fire.performed += OnFired;

        InputManager.playerInputActions.Player.Sprint.performed += OnSprint;
        InputManager.playerInputActions.Player.Sprint.canceled += OnSprint;
        InputManager.playerInputActions.Player.Crouch.performed += OnCrouched;
    }

    public override void StateExited()
    {
        //s.playerInput.Player.Move.performed -= OnMove;
        //s.playerInput.Player.Move.canceled -= OnMove;

        InputManager.playerInputActions.Player.Action.performed -= OnAction;
        InputManager.playerInputActions.Player.Action.canceled -= OnAction;

        InputManager.playerInputActions.Player.Interact.performed -= OnInteract;
        InputManager.playerInputActions.Player.Interact.canceled -= OnInteract;

        InputManager.playerInputActions.Player.Fire.performed -= OnFired;

        InputManager.playerInputActions.Player.Sprint.performed -= OnSprint;
        InputManager.playerInputActions.Player.Sprint.canceled -= OnSprint;
        InputManager.playerInputActions.Player.Crouch.performed -= OnCrouched;

        s.playerAnimator.SetBool("animIsMoving", false);
        s.isSprinting = false;
        s.speed = 8;
    }

    

    public override void Update(float deltaTime)
    {
        s.ApplyGravity(deltaTime);
        //Converts movement input to a float because vector3 cant be lerped :(((((
        float inputMagnitude = Mathf.Clamp(s.movementInput.magnitude,s.minWalkClamp,1.0f);
        //SetWalkAnimSpeed(inputMagnitude);

        if (s.isSprinting && !s.isCrouching)
        {
            //Smoothly blend speed off of joystick input
            s.speed = Mathf.Lerp(s.speed, inputMagnitude * s.sprintSpeed, Time.deltaTime * s.acceleration);
        }
        else if (!s.isSprinting && s.isCrouching)
        {
            s.speed = Mathf.Lerp(s.speed, inputMagnitude * s.crouchSpeed, Time.deltaTime * s.acceleration);
        }
        else if (!s.isSprinting && !s.isCrouching)
        {
            s.speed = Mathf.Lerp(s.speed, inputMagnitude * s.maxWalkSpeed, Time.deltaTime * s.acceleration);
        }
        SetWalkAnimSpeed(s.speed);

        // move!!
        Vector3 movement = new Vector3(s.movementInput.x, 0, s.movementInput.y).normalized * s.speed;

        // gravity!!
        movement.y = s.gravityVelocity;

        // Move the character using the CharacterController
        s.characterController.Move(movement * deltaTime);

        // Rotate the player if moving on the XZ plane
        if (movement.x != 0 || movement.z != 0)
        {
            Quaternion toRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
            s.transform.rotation = Quaternion.RotateTowards(s.transform.rotation, toRotation, 1000 * deltaTime);

            s.playerAnimator.SetBool("animIsMoving", true);
        }
        else
        {
            s.playerAnimator.SetBool("animIsMoving", false);
            s.isSprinting = false;
        }

    }

    private void SetWalkAnimSpeed(float inputMagnitude)
    {
        s.playerAnimator.SetFloat("currentWalkVelocity", inputMagnitude* s.slideSpeedCorrection);
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && !s.isCrouching)
        {
            s.isSprinting = true;
            //s.speed = s.sprintSpeed;
        }
        else if (context.canceled && !s.isCrouching)
        {
            s.isSprinting = false;
        }
    }

    private void OnCrouched(InputAction.CallbackContext context)
    {
        s.isCrouching = !s.isCrouching;
        s.playerAnimator.SetBool("animIsCrouching", s.isCrouching);
        if (s.isCrouching)
        {
            //s.speed = 4;
            s.GetComponent<CapsuleCollider>().center = new Vector3(0, 0, 0);
            s.GetComponent<CapsuleCollider>().height = 2;
        }
        else if(!s.isCrouching && !s.isSprinting)
        {
            s.playerAnimator.SetBool("animIsCrouching", false);
            //s.speed = 8;
            s.GetComponent<CapsuleCollider>().center = new Vector3(0, 1, 0);
            s.GetComponent<CapsuleCollider>().height = 4;
        }
    }


    private void OnFired(InputAction.CallbackContext context)
    {
        s.stateMachine.Enter("PlayerThrowBirdseedState");
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        s.movementInput = context.ReadValue<Vector2>(); // Normal movement
    }

    private void OnAction(InputAction.CallbackContext context)
    {

    }

    private void OnInteract(InputAction.CallbackContext context)
    {
       //if (context.performed)
       //{
            //if (s.qteInteract.playerInRange)
            //{
            //activate when interact key is pressed
            //s.qteInteract.ActivateTimingMeter();
            //}
       //}
    }
}
