using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 1;
    [SerializeField] private LayerMask mask = -1;
    [Header("Runtime")]
    [SerializeField] private float xInput = 0;
    Transform cameraTransform;

    private void Start() {
        cameraTransform = Camera.main.transform;
        xInput = transform.rotation.eulerAngles.y;    
    }
    public void LookAt(Transform target)
    {
        LookAt(target.position);
    }

    public void LookAt(Vector3 target)
    {
        transform.LookAt(target);
        xInput = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, xInput, 0);
    }

    public void SmoothLookAt(Vector3 target, float duration, Ease ease = Ease.InOutQuad)
    {
        Vector3 forward = target - transform.position;
        forward.y = 0;
        Quaternion rotation = Quaternion.LookRotation(forward);
        var tween = transform.DORotateQuaternion(rotation, duration);
        tween.onComplete += () => xInput = transform.rotation.eulerAngles.y;
    }

    Transform obstructed = null;
    // Update is called once per frame
    void LateUpdate()
    {
        xInput += Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(0, xInput, 0);
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - transform.position;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, direction.magnitude, mask)) {
            if (obstructed)
                obstructed.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            obstructed = hit.transform;
                obstructed.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

        } else if (obstructed != null) {
            obstructed.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            obstructed = null;
        }
    } 
}
