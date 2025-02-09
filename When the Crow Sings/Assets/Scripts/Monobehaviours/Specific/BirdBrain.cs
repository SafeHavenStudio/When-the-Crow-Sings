using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBrain : StateMachineComponent
{
    public CharacterController controller;
    public CrowHolder crowHolder;

    public float secondsToPeck;
    public float flyingSpeed = .2f;

    public Animator crowAnimator;

    [HideInInspector]
    public CrowRestPoint restPoint;

    [HideInInspector]
    public bool targetIsTargetNotSpawn; // Determines whether TargetState should focus on birdseed or spawn.
    [HideInInspector]
    public bool idleWaitingAfterPecking; // Determines whether the idle state should be of infinite length or return to dispersal after a short wait.

    [HideInInspector]
    public float timeAllowedToReachBirdseed = 2f; // Time in seconds

    private void Awake()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new CrowIdleState(this), "CrowIdleState");
        stateMachine.RegisterState(new CrowTakeoffState(this), "CrowTakeoffState");
        stateMachine.RegisterState(new CrowTargetState(this), "CrowTargetState");
        stateMachine.RegisterState(new CrowPeckState(this), "CrowPeckState");
    }
    private void Start()
    {
        stateMachine.Enter("CrowIdleState");
        controller.enabled = true;
    }

    [HideInInspector]
    public Vector3 destination;
    [HideInInspector]
    public Vector3 direction;

    public void FlyNavigate_FixedUpdate()
    {
        // if raycast detects surface AND that surface is NOT the destination, then navigate away.
        direction =  (destination - transform.position).normalized*flyingSpeed;
        transform.rotation = Quaternion.LookRotation(direction);
        controller.Move(direction);//targetPosition);
    }

    public void SetTargetAsTarget(bool _target)
    {
        targetIsTargetNotSpawn = _target;
        stateMachine.Enter("CrowTakeoffState");
    }

    public void StillGravity()
    {
        controller.Move(new Vector3(0, -1f, 0));
    }

    public void OnCrowTargetActivated(SignalArguments args)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CrowCocophony, this.transform.position);
        SetTargetAsTarget(true);
    }
    public void OnCrowTargetDeactivated(SignalArguments args)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CrowCocophony, this.transform.position);
        SetTargetAsTarget(false);
    }

    public void SetRestPoint(CrowRestPoint _restPoint)
    {
        restPoint = _restPoint;
        //transform.position = restPoint.position;
        // TODO: Add rotation.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CrowRestPoint>() == restPoint && !targetIsTargetNotSpawn) stateMachine.Enter("CrowIdleState");
        if (other.GetComponent<CrowTarget>() && targetIsTargetNotSpawn)
        {
            other.GetComponent<CrowTarget>().StartDisableCoroutine();
            stateMachine.Enter("CrowPeckState");
        }
        
    }
}