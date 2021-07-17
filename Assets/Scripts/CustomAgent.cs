using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomAgent : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 30f;
    [SerializeField] float arrivalRadius = 1f;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;

    Vector3 facing;

    public bool Warp(Vector3 position)
    {
        return agent.Warp(position);
    }

    public bool Warp(Transform target)
    {
        facing = target.forward;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(facing.x, 0, facing.z));
        transform.rotation = lookRotation;
        return agent.Warp(target.position);
    }

    public void MoveTo(Transform target)
    {
        MoveTo(target.position, target.forward);
    }

    public void MoveTo(Vector3 destination, Vector3 facingDirection)
    {
        agent.SetDestination(destination);
        facing = facingDirection;
        facing.Normalize();
    }
             
    private void RotateTowards() {
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(facing.x, 0, facing.z));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void ResetPath()
    {
        agent.ResetPath();
    }
    void Awake()
    {
    }

    public bool HasReachedDestination => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);

    public bool IsNearEnd => agent.path.corners.Length > 1 && agent.path.corners[1] == agent.destination && agent.remainingDistance < arrivalRadius;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < agent.path.corners.Length - 1; i++) {
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red, Time.deltaTime);
        }
        if (Time.deltaTime == 0)
            return;
        float velocity = agent.velocity.magnitude;
        animator.SetFloat("Velocity", velocity);
        if (facing != Vector3.zero && (IsNearEnd || HasReachedDestination)) {
            RotateTowards();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, arrivalRadius);
    }
}
