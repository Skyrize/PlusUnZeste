using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagesOnTouch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damageAmount = 1;
    [Header("References")]
    [SerializeField] private Collider hitBox = null;
    public UnityEvent onTouch = new UnityEvent();

    private void OnCollisionEnter(Collision other) {
        ContactPoint contactPoint = other.GetContact(0);
        HealthComponent otherHealth = other.gameObject.GetComponent<HealthComponent>();

        if (otherHealth != null && contactPoint.thisCollider == hitBox) {
            otherHealth.ReduceHealth(damageAmount);
            onTouch.Invoke();
        }
        
    }

    private void Awake() {
        if (!hitBox)
            hitBox = GetComponentInChildren<Collider>();
    }
}
