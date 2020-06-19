using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EjectOnTouch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float ejectionForce = 3;
    [Header("References")]
    [SerializeField] private Collider hitBox = null;

    private void OnCollisionEnter(Collision other) {
        ContactPoint contactPoint = other.GetContact(0);
        GameProperty properties = other.gameObject.GetComponent<GameProperty>();
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (properties && rb && contactPoint.thisCollider == hitBox && properties.HasProperty(GameProperty.Property.EJECTABLE)) {
            rb.AddForce(-contactPoint.normal * ejectionForce, ForceMode.Impulse);
        }
    }

    private void Awake() {
        if (!hitBox)
            hitBox = GetComponentInChildren<Collider>();
    }
}
