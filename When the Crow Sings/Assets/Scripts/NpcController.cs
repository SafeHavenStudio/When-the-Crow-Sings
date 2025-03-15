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
        stateMachine.Enter("NpcIdleState");

        originalPosition = transform.position;
    }

    // Being super explicit with the API for designers' sakes, especially since UnityEvents don't seem to support enums.
    public void NpcAnimIdleStart()
    {
        Debug.Log("No i don't want to talk to you go away");
        if (animator != null) animator.SetBool("isTalking",false);
        state = NpcState.IDLE;
    }
    public void NpcAnimTalkStart()
    {
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
}
