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
    [SerializeField] private bool returnHome = false;
    [SerializeField] private bool moving = false;
    [SerializeField] private Transform target = null;

    public void SetDestination(Transform targetDestination)
    {
        target = targetDestination;
        agent.SetDestination(target.position);
        moving = true;
    }

    public void MoveToWaypoint()
    {
        target = WaypointManager.instance.GetRandomWaypoint();
        // onMoveToWaypoint.Invoke(target.position);
        agent.SetDestination(target.position);
        moving = true;
    }

    public void ReturnHome()
    {
        target = WaypointManager.instance.home;
        agent.SetDestination(target.position);
        wander = Random.Range(minWanderAmount, maxWanderAmount + 1);
        moving = true;
        returnHome = true;
    }

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
        Debug.Log($"warp {agent.Warp(WaypointManager.instance.home.position)}");
        GetComponent<SeekTarget>().Respawn();
        ReturnHome();
    }

    // Start is called before the first frame update
    void Start()
    {

        moving = false;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        if (returnHome) {
            idleTime = Random.Range(minHomeTime, maxHomeTime);
            returnHome = false;
        }

       wander = Random.Range(minWanderAmount, maxWanderAmount + 1);
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
        animator.SetFloat("Velocity", agent.velocity.sqrMagnitude);
        if (moving) {
            if (agent.remainingDistance != Mathf.Infinity && agent.pathStatus==NavMeshPathStatus.PathComplete && agent.remainingDistance== 0) {
                Idle();
            } else {
                for (int i = 0; i < agent.path.corners.Length - 1; i++) {
                    Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red, Time.deltaTime);
                }
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
