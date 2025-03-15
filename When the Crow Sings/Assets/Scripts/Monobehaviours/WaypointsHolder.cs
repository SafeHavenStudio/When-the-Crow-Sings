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
            return waypoints[Random.Range(0, waypoints.Count-1)];
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
}
