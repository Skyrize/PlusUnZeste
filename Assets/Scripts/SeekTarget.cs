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
    [SerializeField] private float damages = 1;
    [SerializeField] private float tickRate = .3f;
    [SerializeField] float reactionTime = .4f;
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
    [SerializeField] private RawImage viewUI = null;
    [SerializeField] private OpacityController alertUI = null;
    [SerializeField] private OpacityController hiddenUI = null;
    [SerializeField] private CookerController controller;
    [SerializeField] private Transform leftEye;
    [SerializeField] private Transform rightEye;
    [SerializeField] private Transform head;

    private CustomAgent agent;

    enum Visibility
    {
        VISIBLE,
        HIDDEN,
        LOST
    };
    [Header("Runtime")]
    [SerializeField] private Visibility _state = Visibility.LOST;
    private Visibility state {
        get {
            return _state;
        }

        set {
            canReact = false;
            _state = value;
            if (isActiveAndEnabled)
                StartCoroutine(React());
        }
    }
    [SerializeField] bool canReact = true;
    [SerializeField] private float damageTimer = 0;
    [SerializeField] private float currentWaitTime = 0;
    [SerializeField] private float waitTimer = 0;
    
    private ConstraintSource viewSource;
    private LookAtConstraint viewConstraint;
    WaitForSeconds timer;

    [System.Serializable]
    struct Save
    {
        public Visibility state;
        public float damageTimer;
        public float currentWaitTime;
        public float waitTimer;
    };

    [SerializeField] Save currentSave;

    public void LoadState()
    {
        if (!gameObject.activeInHierarchy)
            return;
        this.state = currentSave.state;
        this.damageTimer = currentSave.damageTimer;
        this.currentWaitTime = currentSave.currentWaitTime;
        this.waitTimer = currentSave.waitTimer;
        BecomeOutOfView();
    }

    public void SaveState()
    {
        if (!gameObject.activeInHierarchy)
            return;
        currentSave.state = this.state;
        currentSave.damageTimer = this.damageTimer;
        currentSave.currentWaitTime = this.currentWaitTime;
        currentSave.waitTimer = this.waitTimer;
    }

    IEnumerator React()
    {
        yield return timer;
        canReact = true;
    }

    private void Awake() {
        // Debug.Log("cook awake");
        timer = new WaitForSeconds(reactionTime);
        viewSource.sourceTransform = target;
        viewSource.weight = 1;
        viewConstraint = head.GetComponent<LookAtConstraint>();
        controller = GetComponent<CookerController>();
        agent = GetComponent<CustomAgent>();
    }

    private void Start() {
        SaveState();
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

    public void UnsetTargetView()
    {
        if (viewConstraint.sourceCount == 1)
            viewConstraint.RemoveSource(0);
    }

    public void SetTargetView()
    {
        if (viewConstraint.sourceCount == 0)
            viewConstraint.AddSource(viewSource);
    }

    void SeeTarget()
    {
        if (state == Visibility.VISIBLE)
            return;
        state = Visibility.VISIBLE;
        viewUI.color = cameraColorOnSee;
        alertUI.SetOpacity(1);
        hiddenUI.SetOpacity(0);
        onSeeTarget.Invoke(target.position);
        SetTargetView();
        controller.enabled = false;
        target.GetComponent<PlayerController>().UpdateVisibility(true);
        // Debug.Log("TARGET IN SIGHT !");
    }

    void MoveTo(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1000f, 1 << 4)) {
            agent.MoveTo(hit.position, position - transform.position);
            Debug.DrawRay(hit.position, Vector3.up * 100, Color.red, Time.deltaTime);
        }
    }

    void ChaseTarget()
    {
        // transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        MoveTo(target.position);
    }

    void BecomeHidden()
    {
        state = Visibility.HIDDEN;
        // Debug.Log("targetHidden start");
        waitTimer = Random.Range(minWait, maxWait);
        currentWaitTime = waitTimer;
        alertUI.SetOpacity(0);
        hiddenUI.SetOpacity(1);
        MoveTo(target.position); // Go To last know position one last time
        onHidden.Invoke();
        viewUI.color = cameraColorOnHidden;
        target.GetComponent<PlayerController>().UpdateVisibility(false);
        // Debug.Log("targetHidden");
    }

    void BecomeOutOfView()
    {
        // Debug.Log("Start Lost !");
        onLost.Invoke();
        // view.transform.rotation = camBaseRotation;
        damageTimer = tickRate;
        hiddenUI.SetOpacity(0);
        alertUI.SetOpacity(0);
        state = Visibility.LOST;
        target.GetComponent<PlayerController>().UpdateVisibility(false);
        UnsetTargetView();
        if (!controller.enabled) {
            controller.enabled = true;
            controller.ResetBehavior();
        }
        // view.transform.parent.transform.rotation = rotation;
        viewUI.color = Color.white;
        // Debug.Log("Lost !");
    }

    void DamageTarget()
    {
        if (damageTimer <= 0) {
            target.GetComponent<HealthComponent>()?.ReduceHealth(damages);
            damageTimer = tickRate;
        } else {
            damageTimer -= Time.deltaTime;
        }
    }

    void Wait()
    {
        if (waitTimer <= 0) {
            BecomeOutOfView();
        } else {
            // head.transform.LookAt(lastKnownPosition);
            waitTimer -= Time.deltaTime;
            float ratio = waitTimer / currentWaitTime;
            viewUI.color = Color.Lerp(Color.white, cameraColorOnHidden, ratio);
            hiddenUI.SetOpacity(ratio);
        }
    }

    public void Respawn()
    {
        // BecomeOutOfView();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        bool targetInView = IsTargetInView();
        // Debug.Log($"in view {targetInView}");
        if (targetInView) {
            if (IsTargetDirectlyVisible()) {
                if (state != Visibility.VISIBLE && canReact) {
                    SeeTarget();
                } else {
                    ChaseTarget();
                    DamageTarget();
                }
            } else if (state == Visibility.VISIBLE && canReact) {
                BecomeHidden();
            }
        } else if (state != Visibility.LOST && canReact) {
            // Debug.Log("Target not in view");
            BecomeOutOfView();
        }
        if (state == Visibility.HIDDEN)
            Wait();
    }
}
