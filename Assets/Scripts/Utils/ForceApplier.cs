using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    [SerializeField] private Vector3 force = Vector3.up;
    [SerializeField] private ForceMode forceMode = ForceMode.VelocityChange;

    public void ApplyForceTo(GameObject target) {
        target.GetComponent<Rigidbody>().AddForce(force, forceMode);
    }
}
