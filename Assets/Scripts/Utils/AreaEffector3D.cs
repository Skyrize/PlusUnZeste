using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffector3D : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private Vector3 force = Vector3.up;
    [SerializeField]
    private ForceMode mode = ForceMode.Acceleration;

    private void OnTriggerStay(Collider other) {
        if (other.GetComponent<Rigidbody>()) {
            other.GetComponent<Rigidbody>().AddForce(force, mode);
        }
    }
}
