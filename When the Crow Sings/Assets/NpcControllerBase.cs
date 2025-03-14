using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

abstract public class NpcControllerBase : StateMachineComponent
{
    [FormerlySerializedAs("enemyWaypointsHolders")]
    public List<WaypointsHolder> WaypointsHolders;

    [HideInInspector]
    public Waypoint currentWaypoint;

    [HideInInspector] public WaypointsHolder currentWaypointHolder;

    public float timeToWaitBetweenWander = 2.0f;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;

    protected void SetUpWaypointsOnStart()
    {
        if (WaypointsHolders == null)
        {
            throw new System.Exception("No waypoint holder assigned!");
        }
        else
        {
            currentWaypointHolder = WaypointsHolders[0];
            currentWaypoint = currentWaypointHolder.waypoints[0];
        }
    }

    public void OnChangeWaypointsTriggered(SignalArguments args)
    {
        if (args.objectArgs[0] == this)
        {
            if (WaypointsHolders.Contains((WaypointsHolder)args.objectArgs[1]))
            {
                currentWaypointHolder = (WaypointsHolder)args.objectArgs[1];
            }
        }
    }
}
