using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 3;
    [SerializeField] private float pivotSpeed = 1;
    [Header("References")]
    private Rigidbody rb = null;
    [SerializeField] private Transform pivot = null;

    [Header("Runtime")]
    [SerializeField] public Vector3 direction = Vector3.zero;
    [SerializeField] public float pivotInput = 0;
    [SerializeField] private Vector3 movement = Vector3.zero;
    [SerializeField] private Vector3 eulerAngleVelocity = Vector3.zero;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnDisable() {
        direction = Vector3.zero;
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
        // rb.rotation = Quaternion.identity;
    }

    // Start is called before the first frame update
    void Start()
    {
        eulerAngleVelocity = new Vector3(0, 100, 0);
    }

    private void FixedUpdate() {
        movement = pivot.forward * direction.z + pivot.right * direction.x;
        movement *= speed;
        rb.AddForce(movement);
        // rb.AddTorque(pivotSpeed * pivot.up * pivotInput);
    }
}
