using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectOnTouch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float ejectionForce = 3;
    [SerializeField] private float ejectionTimer = .3f;
    [Header("References")]
    [SerializeField]
    private List<Collider> hitBoxes;
    [SerializeField] bool canEject = true;

    private IEnumerator ToggleEject()
    {
        canEject = false;
        yield return new WaitForSeconds(ejectionTimer);
        canEject = true;
    }

    private void OnCollisionEnter(Collision other) {
        if (!canEject) {
            return;
        }
        ContactPoint contactPoint = other.GetContact(0);
        GameProperty properties = other.gameObject.GetComponent<GameProperty>();
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

        if (properties && rb && hitBoxes.Contains(contactPoint.thisCollider) == true && properties.HasProperty(GameProperty.Property.EJECTABLE)) {
            
            rb.AddForce(-contactPoint.normal * ejectionForce, ForceMode.Impulse);
        }
    }

    // private void OnTriggerEnter(Collider other) {
    //     GameProperty properties = other.gameObject.GetComponent<GameProperty>();
    //     Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();

    //     if (properties && rb && properties.HasProperty(GameProperty.Property.EJECTABLE)) {
    //         Vector3 direction = other.gameObject.transform.position - transform.position;
    //         direction.Normalize();
    //         rb.AddForce(direction * ejectionForce, ForceMode.Impulse);
    //     }
        
    // }

    private void Awake() {
        if (hitBoxes.Count == 0)
            hitBoxes.AddRange(GetComponentsInChildren<Collider>());
    }
}
