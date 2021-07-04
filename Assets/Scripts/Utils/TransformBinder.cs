using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBinder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool bindPosition = true;
    [SerializeField] private bool bindRotation = false;
    [SerializeField] private bool bindScale = false;
    [Header("References")]
    public Transform target = null;
    private Vector3 positionOffset = Vector3.zero;
    private Quaternion rotationOffset;
    private Vector3 scaleOffset = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        if (bindPosition) positionOffset = transform.position - target.position;
        if (bindRotation) rotationOffset = target.rotation * Quaternion.Inverse(transform.rotation);
        if (bindScale) scaleOffset = transform.localScale - target.localScale;
    }

    private void LateUpdate() {
        if (bindPosition) transform.position = target.position + positionOffset;
        if (bindRotation) transform.rotation = rotationOffset * target.rotation;
        if (bindScale) transform.localScale = target.localScale + scaleOffset;
    }
}
