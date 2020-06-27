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
    static private WaypointManager _instance = null;
    static public WaypointManager instance {
        get {
            if (_instance == null)
                Debug.LogException(new System.Exception("Asking for instance too early (awake)"));
            return WaypointManager._instance;
        }

        set {
            if (_instance) {
                Debug.LogException(new System.Exception("More thand one WaypointManager in the Scene"));
            } else {
                _instance = value;
            }
        }
    }

    private void Awake() {
        instance = this;
    }

    public Transform GetRandomWaypoint()
    {
        return waypoints[Random.Range(0, waypoints.Length)];
    }
}
