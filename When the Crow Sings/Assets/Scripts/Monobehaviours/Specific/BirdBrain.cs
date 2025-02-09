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
    public Transform target;

    [HideInInspector]
    public Vector3 direction;

    public void FlyNavigate_FixedUpdate()
    {
        // if raycast detects surface AND that surface is NOT the destination, then navigate away.
        direction =  (target.position - transform.position).normalized*flyingSpeed;
        transform.rotation = Quaternion.LookRotation(direction);
        controller.Move(direction);//targetPosition);
    }

    public void StartFlyingTowardTarget(Transform _target)
    {
        target = _target;
        stateMachine.Enter("CrowTakeoffState");
    }

    public void ApplyGravityWhileStill()
    {
        controller.Move(new Vector3(0, -1f, 0));
    }

    public void StartFlyingTowardBirdseed(GameObject _target)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CrowCocophony, this.transform.position);
        StartFlyingTowardTarget(_target.transform);
    }
    public void StartFlyingTowardRestPoint()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CrowCocophony, this.transform.position);
        StartFlyingTowardTarget(restPoint.transform);
    }

    public void SetRestPoint(CrowRestPoint _restPoint)
    {
        restPoint = _restPoint;
        //transform.position = restPoint.position;
        // TODO: Add rotation.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CrowRestPoint>() == restPoint && target == restPoint.transform) stateMachine.Enter("CrowIdleState");
        else if (other.GetComponent<CrowTarget>() && target != restPoint.transform)
        {
            other.GetComponent<CrowTarget>().StartActingAsObstacle(secondsToPeck);
            stateMachine.Enter("CrowPeckState");
        }
    }
}