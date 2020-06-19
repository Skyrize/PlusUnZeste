using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 1;
    [Header("Runtime")]
    [SerializeField] private float xInput = 0;

    public void LookAt(Vector3 target)
    {
        transform.LookAt(target);
        xInput = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, xInput, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        xInput += Input.GetAxisRaw("Mouse X");
        transform.rotation = Quaternion.Euler(0, xInput * speed, 0);
    }
}
