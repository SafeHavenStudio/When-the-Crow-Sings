using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : NpcControllerBase,IService
{
    public float patrolSpeed = 4.5f;
    public float pursuitSpeed = 13.0f;

    public Animator enemyAnimator;

    //public float timeToWanderIfNoWaypoint = 4.0f; // I don't think this is being used.
    
    public float timeToBeStunned = 2.0f;
    public float lookAtHeight = 2.5f;

    RaycastHit hit;

    [HideInInspector]
    public bool canSeePlayer = false;

    public EnemySightCone enemySightCone;
    public Transform raycastStart;
    public List<LineRenderer>  lineRenderers;

    bool isWaitingToCheckCanSeePlayer = false;
    public float bufferBeforeSeesPlayer = .2f;
    public bool doesSeePlayer
    {
        get
        {
            return canSeePlayer && enemySightCone.playerInSightCone;
        }
    }
    public bool IsChasingPlayer = false;
    

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        RegisterSelfAsService();
        

        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new EnemyPatrolState(this), "EnemyPatrolState");
        stateMachine.RegisterState(new EnemyChaseState(this), "EnemyChaseState");
        stateMachine.RegisterState(new EnemyStunnedState(this), "EnemyStunnedState");
        stateMachine.RegisterState(new EnemyIdleState(this), "EnemyIdleState");
        stateMachine.Enter("EnemyIdleState");
    }
    private void Start()
    {
        SetUpWaypointsOnStart();

    }

    public void OnSpotPlayerRegardlessTriggerEntered(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            stateMachine.Enter("EnemyChaseState");
    }

    public void EnterChaseStateSafe()
    {
        Debug.Log("EnterChaseStateSafe() called!");
        if (!isWaitingToCheckCanSeePlayer) StartCoroutine(checkIfStillDoesSeePlayer());
    }
    IEnumerator checkIfStillDoesSeePlayer()
    {
        isWaitingToCheckCanSeePlayer = true;
        yield return new WaitForSeconds(bufferBeforeSeesPlayer);
        if (canSeePlayer) stateMachine.Enter("EnemyChaseState");
        isWaitingToCheckCanSeePlayer = false;
    }

    //public void SightConeTriggerEntered(Collider other)
    //{
    //    //stateMachine.OnTriggerEnter(other);
    //}

    private void OnTriggerEnter(Collider other)
    {
        stateMachine.OnTriggerEnter(other); // This is in the StateMachineComponent and shouldn't be duplicated ideally...

        if (other.GetComponent<BirdBrain>())
        {
            stateMachine.Enter("EnemyStunnedState");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.EnemyStun, this.transform.position);
        }
    }
    public void SightConeTriggerExited(Collider other)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }



    //public void SightConeTriggerStay(Collider other)
    //{

    //}

    private void OnDestroy()
    {
        canSeePlayer = false;
        ServiceLocator.Unregister<EnemyController>();
    }

    private void FixedUpdate()
    {
        if ( ServiceLocator.Get<PlayerController>() != null )
        {
            Vector3 targetPosition = ServiceLocator.Get<PlayerController>().transform.position;
            targetPosition.y += lookAtHeight;

            canSeePlayer = false;
            RaycastCheck(targetPosition);
            if (!canSeePlayer)
            {
                targetPosition.y -= 3.0f;
                RaycastCheck(targetPosition); // Check the lower one if the first one didn't see.
            }
        }
        stateMachine.FixedUpdate();
    }
    private void RaycastCheck(Vector3 targetPosition)
    {
        

        Vector3 direction = (targetPosition - raycastStart.position).normalized;
        if (Physics.Raycast(raycastStart.position, direction, out hit, 1000, ~LayerMask.GetMask("Enemy","Interactable","Player")))
        {
            if (DebugManager.showCollidersAndTriggers)
            {
                lineRenderers[0].SetPosition(0, raycastStart.position);
                lineRenderers[0].enabled = true;
                lineRenderers[0].SetPosition(1, hit.point);
            }
            

            if (hit.transform.CompareTag("Player"))
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }

    private void RenderRayCastLine(List<Vector3> targetPositions)
    {
        if (DebugManager.showCollidersAndTriggers)
        {
            foreach (Vector3 pos in targetPositions)
            {
                LineRenderer lineRenderer = lineRenderers[targetPositions.IndexOf(pos)];
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, raycastStart.position);
                lineRenderer.SetPosition(1, pos);
                if (canSeePlayer) lineRenderer.startColor = Color.red;
                else lineRenderer.startColor = Color.green;
            }
        }
        else
        {
            foreach (LineRenderer i in lineRenderers)
            {
                i.enabled = false;
            }
        }
    }

    public void RegisterSelfAsService()
    {
        ServiceLocator.Register(this);
    }

    //private void FixedUpdate()
    //{
    //    RaycastHit hit;

    //    Vector3 targetPosition = ServiceLocator.Get<PlayerController>().transform.position;
    //    targetPosition.y += lookAtHeight;


    //    LineRenderer lineRenderer = GetComponent<LineRenderer>();
    //    lineRenderer.enabled = true;
    //    lineRenderer.SetPosition(0, transform.position);
    //    lineRenderer.SetPosition(1, targetPosition);



    //    if (Physics.Raycast(transform.position, targetPosition - transform.position, out hit))
    //    {
    //        if (hit.transform.tag == "Player")
    //        {
    //            Debug.Log("I SEE YOU");
    //        }
    //    }
    //}
}
