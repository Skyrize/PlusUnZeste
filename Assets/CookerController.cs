using System.Collections;
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
    [SerializeField] private int minWanderAmount = 1;
    [SerializeField] private int maxWanderAmount = 2;

    // [Header("Events")]
    // [SerializeField] private PositionEvent onMoveToWaypoint = new PositionEvent();
    // [SerializeField] private PositionEvent onReturnHome = new PositionEvent();
    // [SerializeField] private UnityEvent onStartIdle = new UnityEvent();

    private NavMeshAgent agent;
    private Animator animator;

    [Header("Runtime")]
    [SerializeField] private float idleTime = 0;
    [SerializeField] private int wander = 0;
    [SerializeField] private bool shouldReturnHome = false;
    [SerializeField] private bool moving = false;
    [SerializeField] private Transform target = null;

    public void MoveToWaypoint()
    {
        target = WaypointManager.instance.GetRandomWaypoint();
        // onMoveToWaypoint.Invoke(target.position);
        agent.SetDestination(target.position);
        animator.SetBool("IsMoving", true);
        moving = true;
    }

    public void ReturnHome()
    {
        target = WaypointManager.instance.home;
        agent.SetDestination(target.position);
        animator.SetBool("IsMoving", true);
        wander = Random.Range(minWanderAmount, maxWanderAmount + 1);
        moving = true;
        shouldReturnHome = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        moving = false;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        if (shouldReturnHome) {
            idleTime = Random.Range(minHomeTime, maxHomeTime);
            shouldReturnHome = false;
        }

       wander = Random.Range(minWanderAmount, maxWanderAmount + 1);
    }

    public void Idle()
    {
        moving = false;
        animator.SetBool("IsMoving", false);
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        if (shouldReturnHome) {
            idleTime = Random.Range(minHomeTime, maxHomeTime);
            shouldReturnHome = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) {
            if (agent.remainingDistance != Mathf.Infinity && agent.pathStatus==NavMeshPathStatus.PathComplete && agent.remainingDistance== 0) {
                Idle();
            }
        } else {
            if (idleTime <= 0) {
                if (wander > 0) {
                    MoveToWaypoint();
                    wander--;
                } else {
                    ReturnHome();
                }
            } else {
                //Idle
                idleTime -= Time.deltaTime;
            }
        }
    }
}
