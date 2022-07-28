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

        if (rb && hitBoxes.Contains(contactPoint.thisCollider) == true /*TODO rework*/ && properties && properties.Ejactable) {
            Debug.Log("Eject");
            StartCoroutine(ToggleEject());
            Vector3 ejection = (rb.transform.position - contactPoint.point).normalized * ejectionForce;
            rb.AddForce(ejection, ForceMode.Impulse);
        }
    }

    private void Awake() {
        if (hitBoxes.Count == 0)
            hitBoxes.AddRange(GetComponentsInChildren<Collider>());
    }
}
