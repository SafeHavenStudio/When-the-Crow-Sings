using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerController : StateMachineComponent, IService
{
    // Paul code
    public Transform throwPosition;
    public GameObject throwTarget;
    public Animator playerAnimator;

    public float timeToFear = 2f;
    public GameSettings gameSettings;


    [SerializeField]
    private BirdseedController pfBirdseedProjectile;
    [HideInInspector]
    public bool isCrouchingToggled = false;

    [HideInInspector] public bool isInteracting = false;

    //[HideInInspector]

    //private bool _isSprinting;
    [HideInInspector]
    public bool isSprintingButtonHeld;

    [HideInInspector]
    public bool isAlwaysSprinting = false;

    //{
    //    set
    //    {
    //        _isSprinting = value;
    //        if (playerAnimator != null) playerAnimator.SetBool("animIsSprinting", value);

    //    }
    //    get { return _isSprinting; }
    //}
    [HideInInspector]
    public float gravity = -9.81f;
    [HideInInspector]
    public float gravityMultiplier = 3f;
    [HideInInspector]
    public float gravityVelocity;

    public float turnSpeed = 10f;

    public float maxWalkSpeed;
    public float minWalkSpeed;
    public float minSprintSpeed;
    public float maxSprintSpeed;
    public float minCrouchSpeed;
    public float maxCrouchSpeed;

    public float walkSlideSpeedCorrection = 0.19f; //Used for walk(?) animation.
    public float crouchSlideSpeedCorrection = 0.19f; //Used for walk(?) animation.
    public float sprintSlideSpeedCorrection = 0.19f; //Used for walk(?) animation.

    public CharacterController characterController;

    public GameObject trajectoryLine;

    public LayerMask SlopeLayerMask;

    public Transform playerLookAtTransform;
    public Transform playerLookAtTransformHolder;

    public GameSignal pauseSignalTEMP;
    public GameSignal mapSignalTEMP;
    public GameSignal historySignalTEMP;
    public GameSignal codexSignalTEMP;

    public void ApplyGravity(float deltaTime)
    {
        //Apply gravity to velocity
        gravityVelocity += gravity * gravityMultiplier * deltaTime;

        if (characterController.isGrounded && gravityVelocity < 0)
        {
            gravityVelocity = 0; //Reset vertical velocity
        }
    }

    Transform spawnTransform;
    public void Initialize(Transform _transform)
    {
        spawnTransform = _transform;
        SnapPlayerToTransform(spawnTransform);
    }

    private void Awake()
    {
        RegisterSelfAsService();

        characterController = GetComponent<CharacterController>();

        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new PlayerFrozenState(this), "PlayerFrozenState");
        stateMachine.RegisterState(new PlayerMovementState(this), "PlayerMovementState");
        stateMachine.RegisterState(new PlayerThrowBirdseedState(this), "PlayerThrowBirdseedState");
        stateMachine.RegisterState(new PlayerFearState(this), "PlayerFearState");
        stateMachine.RegisterState(new PlayerDestroyState(this), "DestroyState");

        gameSettings = FindObjectOfType<GameSettings>(true);

        if (gameSettings != null)
        {
            gameSettings.playerController = this;
            gameSettings.LoadSavedPreferences();
        }

        Debug.Log("Player controller awaken");
    }

    private void Start()
    {
        InputManager.playerInputActions.Player.Enable();
        stateMachine.Enter("PlayerFrozenState");
    }

    private void OnDestroy()
    {
        stateMachine.Enter("DestroyState");
    }

    public void RegisterSelfAsService()
    {
        ServiceLocator.Register<PlayerController>(this);
    }


    public void ThrowBirdseed()
    {
        var direction = throwTarget.transform.position - throwPosition.transform.position;
        direction.y = 4;
        BirdseedController.Create(pfBirdseedProjectile, throwPosition, direction);
    }

    public List<GameObject> playerVisuals;
    public void HidePlayerVisuals(bool _hide = false)
    {
        foreach (GameObject i in playerVisuals)
        {
            i.SetActive(_hide);
        }
    }
    private void OnEnable()
    {
        InputManager.playerInputActions.Player.Pause.performed += OnPause;
        InputManager.playerInputActions.Player.OpenMap.performed += OnMap;
        InputManager.playerInputActions.Player.OpenHistory.performed += OnHistory;
        InputManager.playerInputActions.Player.OpenPeopleThatSoundsSOWrong.performed += OnCodex;
    }
    private void OnDisable()
    {
        InputManager.playerInputActions.Player.Pause.performed -= OnPause;
        InputManager.playerInputActions.Player.OpenMap.performed -= OnMap;
        InputManager.playerInputActions.Player.OpenHistory.performed -= OnHistory;
        InputManager.playerInputActions.Player.OpenPeopleThatSoundsSOWrong.performed -= OnCodex;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        pauseSignalTEMP.Emit();
    }
    private void OnMap(InputAction.CallbackContext context)
    {
        mapSignalTEMP.Emit();
    }
    private void OnHistory(InputAction.CallbackContext context)
    {
        historySignalTEMP.Emit();
    }
    private void OnCodex(InputAction.CallbackContext context)
    {
        codexSignalTEMP.Emit();
    }




    [HideInInspector] public Interactable mostRecentInteractable;
    public void OnInteractStarted(SignalArguments signalArgs)
    {
        mostRecentInteractable = (Interactable)signalArgs.objectArgs[0];

        switch (mostRecentInteractable.playerResponse)
        {
            case Interactable.PlayerResponses.NONE:
                Debug.Log("No reason to stop.");
                break;
            case Interactable.PlayerResponses.FREEZE:
                isInteracting = true;
                stateMachine.Enter("PlayerFrozenState");
                break;
            case Interactable.PlayerResponses.FEAR:
                stateMachine.Enter("PlayerFearState");
                break;
            case Interactable.PlayerResponses.FREEZE_AND_TALK:
                // TODO: Make Chane actually have a talk animation.
                isInteracting = true;
                stateMachine.Enter("PlayerFrozenState");
                break;
        }
        
        if (mostRecentInteractable.playerSnapPoint != null)
        {
            SnapPlayerToTransform(mostRecentInteractable.playerSnapPoint);
        }
        else if (mostRecentInteractable.playerFacesInteractable)
        {
            SnapPlayerToTransform(mostRecentInteractable.transform, true);
        }
    }

    [SerializeField] PlayerInteractionArea playerInteractionArea;
    public void OnInteractFinished(SignalArguments signalArgs)
    {
        isInteracting = false;
        playerInteractionArea.StartCooldown();

        HidePlayerVisuals(true);
        stateMachine.Enter("PlayerMovementState");
    }

    public void SnapPlayerToTransform(Transform _transform, bool onlyRotation = false)
    {
        characterController.enabled = false;
        if (onlyRotation)
        {
            Vector3 direction = _transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            //transform.SetPositionAndRotation(transform.position, _transform.rotation);
        }
        else transform.SetPositionAndRotation(_transform.position, _transform.rotation);
        characterController.enabled = true;
    }

    public void OnFullyLoadFinished(SignalArguments args)
    {
        if (!ServiceLocator.Get<DialogueManager>().isInDialogue)
        {
            stateMachine.Enter("PlayerMovementState");
        }
        //SnapPlayerToTransform(spawnTransform);
    }
    public void OnAnimationFinished(SignalArguments args)
    {
        if (args.stringArgs[0] == "Throw")
        {
            stateMachine.Enter("PlayerMovementState");
        }
    }


    // Ricky code
    public float speed;
    [HideInInspector]
    public Vector3 movementInput;
    [HideInInspector]
    public float acceleration = 5f;

}