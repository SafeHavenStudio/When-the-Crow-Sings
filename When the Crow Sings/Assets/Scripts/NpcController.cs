using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : NpcControllerBase
{
    public Animator animator;

    enum NpcState { IDLE, TALKING }
    NpcState state = NpcState.IDLE;

    [HideInInspector]
    public bool canWander
    {
        get
        {
            return SaveDataAccess.saveData.boolFlags[flagToEnableWandering];
        } 
    }
    public string flagToEnableWandering = "AlwaysFalse";


    public float walkSpeed = 1.0f;

    public Waypoint destinationForDialogueTriggeredMovement;
    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();
        GetComponent<NavMeshAgent>().radius = GetComponent<CapsuleCollider>().radius;
        GetComponent<NavMeshAgent>().height = GetComponent<CapsuleCollider>().height;
        navMeshAgent.speed = walkSpeed;

        SetUpWaypointsOnStart();

        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new NpcIdleState(this), "NpcIdleState");
        stateMachine.RegisterState(new NpcPatrolState(this), "NpcPatrolState");
        stateMachine.RegisterState(new NpcBespokeDestinationState(this), "NpcBespokeDestinationState");
        stateMachine.RegisterState(new NpcReturnToOriginalPositionState(this), "NpcReturnToOriginalPositionState");
        stateMachine.Enter("NpcIdleState");

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    // Being super explicit with the API for designers' sakes, especially since UnityEvents don't seem to support enums.
    public void NpcAnimIdleStart()
    {
        Debug.Log("No i don't want to talk to you go away");
        if (animator != null) animator.SetBool("isTalking",false);
        state = NpcState.IDLE;

        transform.rotation = originalRotation;

        stateMachine.Enter("NpcIdleState");
    }
    public void NpcAnimTalkStart()
    {
        transform.rotation = Quaternion.LookRotation((ServiceLocator.Get<PlayerController>().transform.position - transform.position), Vector3.up);

        Debug.Log("I am playing a talking animation now!");
        if (animator != null)  animator.SetBool("isTalking", true);
        state = NpcState.TALKING;
    }

    public void OnDialogueEnd(SignalArguments args)
    {
        switch (state)
        {
            case NpcState.IDLE:
                break;
            case NpcState.TALKING:
                NpcAnimIdleStart();
                break;
        }
    }

    public void OnNpcWalkSignal(SignalArguments args)
    {
        if (destinationForDialogueTriggeredMovement != null)
        {
            stateMachine.Enter("NpcBespokeDestinationState");
        }
        else
        {
            throw new Exception("Signal emitted for NPC to walk, but there's no destination for dialogue-triggered movement!");
        }
    }


    bool shouldReturnToPosition = false;
    private void OnEnable()
    {
        if (shouldReturnToPosition) enterNewState();
        shouldReturnToPosition = false;

        void enterNewState()
        {
            if (destinationForDialogueTriggeredMovement != null)
            {
                stateMachine.Enter("NpcReturnToOriginalPositionState");
            }
            else
            {
                throw new Exception("Signal emitted for NPC to walk, but there's no destination for dialogue-triggered movement!");
            }
        }
    }
    public void OnNpcWalkSignal2(SignalArguments args)
    {
        Debug.Log("Returning!");
        shouldReturnToPosition = true;
        SaveDataAccess.SetFlag(GetComponent<DynamicEnable>().associatedDataKey, GetComponent<DynamicEnable>().boolValue);
    }
}
