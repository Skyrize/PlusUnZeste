using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Floater : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float depthBeforeSubmerged = 1f;
    [SerializeField] private float displacementAmount = 1f;
    [SerializeField] private int floaterCount = 1;
    [SerializeField] private float waterDrag = 1f;
    [SerializeField] private float waterAngularDrag = 1f;

    
    [Header("Events")]
    [SerializeField] private UnityEvent onDive = new UnityEvent();
    [SerializeField] private UnityEvent onEmerge = new UnityEvent();

    [HideInInspector] public Transform waterPlane = null;

    private Rigidbody rb = null;
    private bool baseGravity = false;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        baseGravity = rb.useGravity;
    }

    public void Dive(Transform waterPlaneTransform)
    {
        Debug.Log("Immerge");
        waterPlane = waterPlaneTransform;
        rb.useGravity = false;
        onDive.Invoke();
    }

    public void Emerge()
    {
        Debug.Log("Emerge");
        waterPlane = null;
        rb.useGravity = baseGravity;
        onEmerge.Invoke();
    }

    void Float()
    {
        float displacementMultiplier = Mathf.Clamp01((waterPlane.position.y - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
        rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
        rb.AddForce(displacementMultiplier * -rb.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddTorque(displacementMultiplier * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void FixedUpdate() {
        if (waterPlane != null) {
            Debug.Log("Emergé");
            rb.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

            if (transform.position.y < waterPlane.position.y) {
                Debug.Log("Below surface");
                Float();
            } else {
                
                Debug.Log("on top fo surface");
            }
        }
    }
}
