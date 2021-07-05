using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastPlacer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float raycastRange = 2f;
    [SerializeField] private float offset = 0.05f;
    [SerializeField] private LayerMask mask = -1;

    public void Replace()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * raycastRange / 2f, -Vector3.up, out hit, raycastRange, mask)) {
            transform.position = hit.point + Vector3.up * offset;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up * raycastRange / 2f, -Vector3.up * raycastRange);
    }
}
