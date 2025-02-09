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
    [HideInInspector] public Vector3 approachPoint;

    [HideInInspector]
    public Vector3 direction;

    [HideInInspector]
    public float progressToTarget = 0f;
    float progressSpeed = .1f;
    float approachWeight = 1f;
    public void FlyNavigate_FixedUpdate()
    {
        // if raycast detects surface AND that surface is NOT the destination, then navigate away.
        //direction =  (target.position - transform.position).normalized*flyingSpeed;

        
        
        
        //transform.rotation = Quaternion.LookRotation(direction);

        progressToTarget = Mathf.Clamp01(progressToTarget + (1.0f / 60.0f)*progressSpeed);
        Debug.Log("Progress: " + progressToTarget.ToString());

        Vector3 weights = new Vector3(1f - progressToTarget, approachWeight * (-Mathf.Pow(((progressToTarget * 2f) - 1f), 4f) + 1), 0 + progressToTarget);
        //Vector3 weights = new Vector3(0,1,0);
        weights = weights.normalized;

        Vector3 weightedWayPoint = 
            ((restPoint.transform.position * weights.x)
            + (approachPoint * weights.y)
            + (target.position * weights.z))
            / (weights.x + weights.y + weights.z);

        debugWaypoint.position = weightedWayPoint;

        direction = (weightedWayPoint - transform.position).normalized * flyingSpeed;

        controller.Move(direction);//targetPosition);
    }
    public Transform debugWaypoint;
    public AnimationCurve weightBCurve;

    public void StartFlyingTowardTarget(Transform _target)
    {
        target = _target;
        progressToTarget = 0f;
        stateMachine.Enter("CrowTakeoffState");
    }

    public void ApplyGravityWhileStill()
    {
        controller.Move(new Vector3(0, -1f, 0));
    }

    public void StartFlyingTowardBirdseed(GameObject _target)
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CrowCocophony, this.transform.position);

        approachPoint = _target.GetComponent<CrowSubTarget>().FindApproachPoint();
        StartFlyingTowardTarget(_target.transform);
    }
    public void StartFlyingTowardRestPoint()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.CrowCocophony, this.transform.position);

        approachPoint = restPoint.approachPoint.position;
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