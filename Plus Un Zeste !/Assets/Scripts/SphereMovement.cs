using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 3;
    [Header("References")]
    private Rigidbody rb = null;
    [SerializeField] private Transform pivot = null;

    [Header("Runtime")]
    [SerializeField] public Vector3 direction = Vector3.zero;
    [SerializeField] private Vector3 movement = Vector3.zero;
    [SerializeField] private Vector3 eulerAngleVelocity = Vector3.zero;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        eulerAngleVelocity = new Vector3(0, 100, 0);
    }

    private void FixedUpdate() {
        movement = direction * speed;
        direction = pivot.forward * movement.z + pivot.right * movement.x;
        rb.AddForce(movement);
    }
}
