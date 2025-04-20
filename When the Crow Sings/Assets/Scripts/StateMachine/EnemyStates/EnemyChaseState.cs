using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyController component) : base(component)
    {
    }

    public override void FixedUpdate()
    {
        if (SaveDataAccess.saveData.boolFlags["EnemyCanMove"] == false)
        {
            s.stateMachine.Enter("EnemyIdleState");
        }
        if (ServiceLocator.Get<PlayerController>() != null)
            s.navMeshAgent.destination = ServiceLocator.Get<PlayerController>().transform.position;
    }

    public override void OnTriggerExit(Collider other)
    {
        //s.stateMachine.Enter("EnemyPatrolState");
    }
    public override void StateEntered()
    {
        s.navMeshAgent.speed = s.pursuitSpeed;
        s.enemyAnimator.SetBool("animChase", true);
        s.IsChasingPlayer = true;
    }
    public override void StateExited()
    {
        s.enemyAnimator.SetBool("animChase", false);
        s.IsChasingPlayer = false;
    }
}
