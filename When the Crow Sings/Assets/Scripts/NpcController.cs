using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : NpcControllerBase
{
    public Animator animator;

    public DialoguePortraits.WhoIsTalking npcIdentity = DialoguePortraits.WhoIsTalking.None;

    public enum NpcState { IDLE, TALKING }
    [HideInInspector]
    public NpcState talkingState = NpcState.IDLE;

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
        if (navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>(); // Null check is probably not necessary but hey, doesn't meaningfully hurt anything.
        navMeshAgent.radius = GetComponent<CapsuleCollider>().radius;
        navMeshAgent.height = GetComponent<CapsuleCollider>().height;
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

    private void Update()
    {
        stateMachine.Update(Time.deltaTime);

        if (animator != null) animator.SetBool("isTalking", DialogueManager.whoIsTalking == npcIdentity && talkingState == NpcState.TALKING);
    }

    // Being super explicit with the API for designers' sakes, especially since UnityEvents don't seem to support enums.
    public void NpcAnimIdleStart()
    {
        if(stateMachine.currentState != stateMachine.states["NpcBespokeDestinationState"])
        {
            Debug.Log("No i don't want to talk to you go away");
            //if (animator != null) animator.SetBool("isTalking", false);
            talkingState = NpcState.IDLE;

            transform.rotation = originalRotation;

            stateMachine.Enter("NpcIdleState");
        }
    }
    public void NpcAnimTalkStart()
    {
        Vector3 direction = ServiceLocator.Get<PlayerController>().transform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        Debug.Log("I am playing a talking animation now!");
        //if (animator != null)  animator.SetBool("isTalking", true);

        talkingState = NpcState.TALKING;

        stateMachine.Enter("NpcIdleState");
    }

    public void OnDialogueEnd(SignalArguments args)
    {
        switch (talkingState)
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
