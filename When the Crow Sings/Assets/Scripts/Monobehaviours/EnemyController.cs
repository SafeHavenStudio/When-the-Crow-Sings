using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : StateMachineComponent
{
    public MeshCollider sightCone;

    private void Awake()
    {
        stateMachine = new StateMachine();
        stateMachine.RegisterState(new EnemyPatrolState(this), "EnemyPatrolState");
        stateMachine.Enter("EnemyPatrolState");
    }
}