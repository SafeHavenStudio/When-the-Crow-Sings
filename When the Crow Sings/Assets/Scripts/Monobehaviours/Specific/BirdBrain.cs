using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBrain : StateMachineComponent
{
    public CharacterController controller;
    public CrowHolder crowHolder;

    public LayerMask whenFlyingMask;

    public float secondsToPeck;
    public float flyingSpeed = .2f;

    public Animator crowAnimator;

    [HideInInspector]
    public CrowRestPoint restPoint;

    public CrowRestPoint.CrowTypes crowType = CrowRestPoint.CrowTypes.REGULAR;

    public FearInteractable fearInteractable;

    [HideInInspector]
    public bool idleWaitingAfterPecking; // Determines whether the idle state should be of infinite length or return to dispersal after a short wait.

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

    public void Initialize(CrowHolder _crowHolder, CrowRestPoint _restPoint)
    {
        crowHolder = _crowHolder;
        SetRestPoint(_restPoint);
        crowType = _restPoint.crowType;
    }

    [HideInInspector]
    public Transform target;
    [HideInInspector] public Vector3 approachPoint;

    [HideInInspector]
    public Vector3 direction;

    [HideInInspector]
    public float progressToTarget = 0f;
    float progressSpeed = .5f;
    float approachWeight = 1f;
    public void FlyNavigate_FixedUpdate()
    {
        // if raycast detects surface AND that surface is NOT the destination, then navigate away.

        if ((transform.position - approachPoint).magnitude < 1)
        {
            crowAnimator.SetBool("isFlying", false);
            progressToTarget = 1f;
        }

        Vector3 weights = new Vector3(weightACurve.Evaluate(progressToTarget), approachWeight * weightBCurve.Evaluate(progressToTarget), weightCCurve.Evaluate(progressToTarget));

        Vector3 weightedWayPoint = 
            ((restPoint.transform.position * weights.x)
            + (approachPoint * weights.y)
            + (target.position * weights.z))
            / (weights.x + weights.y + weights.z);

        direction = (weightedWayPoint - transform.position).normalized * flyingSpeed;

        transform.rotation = Quaternion.LookRotation(direction);

        controller.Move(direction);
    }
    public AnimationCurve weightACurve;
    public AnimationCurve weightBCurve;
    public AnimationCurve weightCCurve;
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