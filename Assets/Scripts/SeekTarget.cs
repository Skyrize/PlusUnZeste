using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.AI;

public class SeekTarget : MonoBehaviour
{
    [Header("Settings")]
    
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

    [Header("Runtime")]
    [SerializeField] private bool targetHidden = false;
    [SerializeField] private bool seeTarget = false;
    [SerializeField] private float damageTimer = 0;
    
    private Quaternion camBaseRotation = Quaternion.identity;

    private void Start() {
        camBaseRotation = view.transform.rotation;
    }

    bool IsTargetInView()
    {
        Vector3 visTest = view.WorldToViewportPoint(target.transform.position);
        return (visTest.x >= 0 && visTest.y >= 0) && (visTest.x <= 1 && visTest.y <= 1) && visTest.z >= 0;
    }

    void SeeTarget()
    {
        if (seeTarget == false) {
            seeTarget = true;
            targetHidden = false;
            displayView.color = cameraColorOnSee;

            onSeeTarget.Invoke(target.position);
            
            GetComponent<Animator>().SetBool("IsMoving", true);
        }
        // Debug.Log("see !");
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        view.transform.LookAt(target.position);
        GetComponent<NavMeshAgent>().SetDestination(target.position);

    }

    void BecomeHidden()
    {
        if (seeTarget && targetHidden == false) {
            targetHidden = true;
            seeTarget = false;
            // Debug.Log("targetHidden start");
            onHidden.Invoke();
            displayView.color = cameraColorOnHidden;
            GetComponent<Animator>().SetBool("IsMoving", false);
        }
        // Debug.Log("targetHidden");
    }

    void BecomeOutOfView()
    {
        if (seeTarget || targetHidden) {
            // Debug.Log("Start Lost !");
            onLost.Invoke();
            // view.transform.rotation = camBaseRotation;
            damageTimer = 0;
            seeTarget = false;
            targetHidden = false;
            // view.transform.parent.transform.rotation = rotation;
            displayView.color = Color.white;
            // GetComponent<Animator>().SetBool("IsMoving", false);
        }
        // Debug.Log("Lost !");
    }

    void DamageTarget()
    {
        if (damageTimer <= 0) {
            target.GetComponent<HealthComponent>().ReduceHealth(damagesPerSeconds);
            damageTimer = 1;
        } else {
            damageTimer -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (IsTargetInView()) {
            Vector3 dist = target.position - view.transform.position;
            RaycastHit hit;
            if (Physics.Raycast(view.transform.position, dist, out hit, dist.sqrMagnitude, mask)) {
                if (hit.transform == target) {
                    SeeTarget();
                } else {
                    BecomeHidden();
                }
            }
        } else {
            BecomeOutOfView();
        }

        if (seeTarget) {
            DamageTarget();
        }
    }
}
