using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChangeWaypointsTrigger : MonoBehaviour
{
    public WaypointsHolder newWaypointHolder;
    public EnemyController enemyToAffect;

    public GameSignal enemyChangeWaypointSignal;

    public void EmitChangeWaypointSignal()
    {
        SignalArguments args  = new SignalArguments();
        args.objectArgs.Add(enemyToAffect);
        args.objectArgs.Add(newWaypointHolder);
        enemyChangeWaypointSignal.Emit(args);
    }
}
