using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform[] waypoints = null;
    [SerializeField] private Transform _home = null;

    public Transform home {
        get {
            return _home;
        }
    }
    [SerializeField]
    int currentWaypoint = 0;

    public Transform GetRandomWaypoint()
    {
        return waypoints[Random.Range(0, waypoints.Length)];
    }

    public Transform GetNextWaypoint()
    {
        Transform result = waypoints[currentWaypoint];

        currentWaypoint++;
        if (currentWaypoint == waypoints.Length)
            currentWaypoint = 0;
        return result;
    }
}
