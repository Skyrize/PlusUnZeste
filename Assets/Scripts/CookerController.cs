﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[System.Serializable]
public class PositionEvent : UnityEvent<Vector3>
{
}

public class CookerController : MonoBehaviour
{
    [Header("Time")]
    [Header("Settings")]
    [SerializeField] private float minIdleTime = 5;
    [SerializeField] private float maxIdleTime = 8;
    [SerializeField] private float minHomeTime = 5;
    [SerializeField] private float maxHomeTime = 8;

    [Header("Wanders")]
    [SerializeField] private bool randomizeWaypoint = false;
    [SerializeField] private int minWanderAmount = 1;
    [SerializeField] private int maxWanderAmount = 2;

    // [Header("Events")]
    // [SerializeField] private PositionEvent onMoveToWaypoint = new PositionEvent();
    // [SerializeField] private PositionEvent onReturnHome = new PositionEvent();
    // [SerializeField] private UnityEvent onStartIdle = new UnityEvent();

    [SerializeField] private CustomAgent agent;

    [Header("Runtime")]
    [SerializeField] private float idleTime = 0;
    [SerializeField] private int wander = 0;
    [SerializeField] private bool returnHome = false;
    [SerializeField] private bool moving = false;
    [SerializeField] private Transform target = null;
    WaypointManager waypointManager;

    public void SetDestination(Transform targetDestination)
    {
        target = targetDestination;
        agent.MoveTo(target);
        moving = true;
    }

    public void MoveToWaypoint()
    {
        if (randomizeWaypoint) {
            SetDestination(waypointManager.GetRandomWaypoint());
        } else {
            SetDestination(waypointManager.GetNextWaypoint());
        }
    }

    public void ReturnHome()
    {
        target = waypointManager.home;
        agent.MoveTo(target);
        wander = Random.Range(minWanderAmount, maxWanderAmount + 1);
        moving = true;
        returnHome = true;
    }

    private void Awake() {

        agent = GetComponent<CustomAgent>();
        waypointManager = FindObjectOfType<WaypointManager>();
        moving = false;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        if (returnHome) {
            idleTime = Random.Range(minHomeTime, maxHomeTime);
            target = waypointManager.home;
            returnHome = false;
        }
        wander = Random.Range(minWanderAmount, maxWanderAmount + 1);
        var home = waypointManager.home;
        agent.Warp(home);
    }

    public void SetIdleTime()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
    }
    public void ResetIdleTime()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        if (returnHome) {
            idleTime = Random.Range(minHomeTime, maxHomeTime);
        }
    }

    public void ResetBehavior()
    {
        SetDestination(target);
    }

    public void Respawn()
    {
        Debug.Log($"warp {agent.Warp(waypointManager.home.position)}");
        GetComponent<SeekTarget>().Respawn();
        ReturnHome();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Idle()
    {
        moving = false;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        if (returnHome) {
            idleTime = Random.Range(minHomeTime, maxHomeTime);
            returnHome = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (moving) {
            if (agent.HasReachedDestination) {
                Idle();
            }
        } else {
            if (idleTime <= 0) {
                if (wander > 0) {
                    MoveToWaypoint();
                    wander--;
                } else {
                    Debug.Log("Go home");
                    ReturnHome();
                }
            } else {
                //Idle
                idleTime -= Time.deltaTime;
            }
        }
    }
}
