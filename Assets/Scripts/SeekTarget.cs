using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.Animations;

public class SeekTarget : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float minWait = 3;
    [SerializeField] private float maxWait = 5;
    [SerializeField] private float damagesPerSeconds = 1;
    [SerializeField] private LayerMask mask = 0;
    [SerializeField] private Color cameraColorOnSee = new Color(1, 0.25f, 0.25f, 1);
    [SerializeField] private Color cameraColorOnHidden = new Color(1, 0.25f, 0.25f, 1);

    [Header("Events")]
    [SerializeField] private PositionEvent onSeeTarget = new PositionEvent();
    [SerializeField] private UnityEvent onHidden = new UnityEvent();
    [SerializeField] private UnityEvent onLost = new UnityEvent();

    [Header("References")]
    [SerializeField] private Camera view = null;
    [SerializeField] private Transform target = null;
    [SerializeField] private RawImage displayView = null;
    [SerializeField] private CookerController controller;
    [SerializeField] private Transform leftEye;
    [SerializeField] private Transform rightEye;
    [SerializeField] private Transform head;

    [SerializeField] private Transform viewTarget;
    private Animator animator;
    private NavMeshAgent agent;

    enum Visibility
    {
        VISIBLE,
        HIDDEN,
        LOST
    };
    [Header("Runtime")]
    [SerializeField] private Visibility state = Visibility.LOST;
    [SerializeField] private float damageTimer = 0;
    [SerializeField] private float waitTimer = 0;
    
    private Quaternion camBaseRotation = Quaternion.identity;
    private Plane[] planes = new Plane[6];
    private ConstraintSource viewSource;
    private LookAtConstraint viewConstraint;

    private void Start() {
        viewSource.sourceTransform = viewTarget;
        viewSource.weight = 1;
        viewConstraint = head.GetComponent<LookAtConstraint>();
        controller = GetComponent<CookerController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        camBaseRotation = view.transform.rotation;
    }

    bool IsTargetInView()
    {
        // GeometryUtility.CalculateFrustumPlanes(view, planes);
        // return GeometryUtility.TestPlanesAABB(planes, target.GetComponent<SphereCollider>().bounds);
        Vector3 visTest = view.WorldToViewportPoint(target.transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }

    bool IsTargetDirectlyVisible(Transform eye)
    {
        Vector3 dist = target.position - eye.position;
        RaycastHit hit;
        if (Physics.Raycast(eye.position, dist, out hit, dist.sqrMagnitude, mask)) {
            // Debug.Log($"{hit.transform.name} in front");
            if (hit.transform == target) {
                Debug.DrawLine(eye.position, target.position, Color.green, Time.deltaTime);
            } else {
                Debug.DrawLine(eye.position, target.position, Color.red, Time.deltaTime);
            }
            return hit.transform == target;
        }
        return false;
    }

    bool IsTargetDirectlyVisible()
    {
        return IsTargetDirectlyVisible(leftEye) || IsTargetDirectlyVisible(rightEye);
    }

    void SeeTarget()
    {
        if (state == Visibility.VISIBLE)
            return;
        state = Visibility.VISIBLE;
        displayView.color = cameraColorOnSee;
        onSeeTarget.Invoke(target.position);
        if (viewConstraint.sourceCount == 0)
            viewConstraint.AddSource(viewSource);
        controller.enabled = false;
        Debug.Log("TARGET IN SIGHT !");
    }

    void ChaseTarget()
    {
        // transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        viewTarget.position = target.position;
        // NavMeshHit hit;
        
        agent.SetDestination(target.position);
        // if (NavMesh.SamplePosition(target.position, out hit, 1000f, 1 << 4)) {
        //     agent.SetDestination(hit.position);
        //     Debug.DrawRay(hit.position, Vector3.up * 100, Color.red, Time.deltaTime);
        // }
    }

    void BecomeHidden()
    {
        state = Visibility.HIDDEN;
        // Debug.Log("targetHidden start");
        waitTimer = Random.Range(minWait, maxWait);
        lastKnownPosition = target.position;
        agent.ResetPath();
        onHidden.Invoke();
        displayView.color = cameraColorOnHidden;
        Debug.Log("targetHidden");
    }

    private Vector3 lastKnownPosition;
    void BecomeOutOfView()
    {
        // Debug.Log("Start Lost !");
        onLost.Invoke();
        // view.transform.rotation = camBaseRotation;
        damageTimer = 0;
        state = Visibility.LOST;
        if (viewConstraint.sourceCount == 1)
            viewConstraint.RemoveSource(0);
        if (!controller.enabled) {
            controller.enabled = true;
            controller.ResetBehavior();
        }
        // view.transform.parent.transform.rotation = rotation;
        displayView.color = Color.white;
        Debug.Log("Lost !");
    }

    void DamageTarget()
    {
        if (damageTimer <= 0) {
            target.GetComponent<HealthComponent>()?.ReduceHealth(damagesPerSeconds);
            damageTimer = 1;
        } else {
            damageTimer -= Time.deltaTime;
        }
    }

    void Wait()
    {
        if (waitTimer <= 0) {
            if (viewConstraint.sourceCount == 1)
                viewConstraint.RemoveSource(0);
            controller.ResetBehavior();
        } else {
            // head.transform.LookAt(lastKnownPosition);
            waitTimer -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        bool targetInView = IsTargetInView();
        Debug.Log($"in view {targetInView}");
        if (targetInView) {
            if (IsTargetDirectlyVisible()) {
                if (state != Visibility.VISIBLE) {
                    SeeTarget();
                } else {
                    ChaseTarget();
                    DamageTarget();
                }
            } else if (state == Visibility.VISIBLE) {
                BecomeHidden();
            }
        } else if (state == Visibility.HIDDEN) {
            Debug.Log("Target not in view");
            BecomeOutOfView();
        }
        animator.SetFloat("Velocity", agent.velocity.sqrMagnitude);
        if (state == Visibility.HIDDEN)
            Wait();
    }
}
