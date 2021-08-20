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
    public int currentWaypoint = 0;

    private void Start() {
        List <Transform> _waypoints = new List<Transform>();
        for (int i = 0; i != transform.childCount; i++) {
            var child = transform.GetChild(i);
            if (child.name != "Home" && child.gameObject.activeInHierarchy)
                _waypoints.Add(child);
        }
        waypoints = new Transform[_waypoints.Count];
        for (int i = 0; i != _waypoints.Count; i++)
        {
            waypoints[i] = _waypoints[i];
        }
    }

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
