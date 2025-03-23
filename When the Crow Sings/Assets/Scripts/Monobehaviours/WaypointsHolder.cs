using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointsHolder : MonoBehaviour
{
    [HideInInspector]
    public List<Waypoint> waypoints;

    public bool randomizeOrder = false;


    private void Awake()
    {
        waypoints = GetComponentsInChildren<Waypoint>().ToList();
        //Debug.Log("Waypoints are " + waypoints);
    }
    private void Start()
    {
        //waypoints = GetComponentsInChildren<EnemyWaypoint>().ToList();
        //Debug.Log("Waypoints are "+waypoints);
    }

    public Waypoint GetNextWaypoint(Waypoint currentWaypoint)
    {
        if (randomizeOrder)
        {
            Waypoint _randomWaypoint;
            _randomWaypoint = returnNewRandomWaypoint(currentWaypoint);
            return _randomWaypoint;
        }
        else
        {
            int currentIndex = waypoints.IndexOf(currentWaypoint);
            currentIndex += 1;
            if (currentIndex > waypoints.Count - 1)
            {
                currentIndex = 0;
            }
            else if (currentIndex < 0)
            {
                currentIndex = waypoints.Count - 1;
            }
            return waypoints[currentIndex];
        }
        
    }

    Waypoint returnNewRandomWaypoint(Waypoint currentWaypoint)
    {
        List<Waypoint> validWaypoints = new List<Waypoint>(waypoints);

        if (validWaypoints.Count > 1) validWaypoints.Remove(currentWaypoint);
        return validWaypoints[Random.Range(0, validWaypoints.Count - 1)];
    }
}
