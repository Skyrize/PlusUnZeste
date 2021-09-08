using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SphereMovement movement = null;
    [SerializeField] private MeshRenderer altMesh = null;
    [SerializeField] JumpComponent jump;
    [SerializeField] bool isBumped = false;
    [SerializeField] float disableDuration = .5f;
    [SerializeField] float currentDisableTimer;
    public bool IsBumped {
        get {
            return isBumped;
        }

        set {
            // Debug.Log("Set");
            isBumped = value;
            if (isBumped)
                currentDisableTimer = disableDuration;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            GetComponent<HealthComponent>().SetMaxHealth(1000000);
        if (isBumped) {
            movement.direction = Vector3.zero;
            if (currentDisableTimer > 0) {
                currentDisableTimer -= Time.deltaTime;
                return;
            }
            if (jump.IsGrounded) {
                isBumped = false;
            }
        } else {
            // movement.direction.x = Input.GetAxis("Horizontal");
            // movement.direction.z = Input.GetAxis("Vertical");
            // movement.direction.Normalize();
            float forward = Input.GetKey(InputSaveManager.instance.GetKey("Forward")) ? 1 : 0;
            float backward = Input.GetKey(InputSaveManager.instance.GetKey("Backward")) ? 1 : 0;
            float left = Input.GetKey(InputSaveManager.instance.GetKey("Left")) ? 1 : 0;
            float right = Input.GetKey(InputSaveManager.instance.GetKey("Right")) ? 1 : 0;
            movement.direction.x = right - left;
            movement.direction.z = forward - backward;
            movement.direction.Normalize();
            // movement.pivotInput = Input.GetAxis("Pivot");
        }
    }

    public void UpdateVisibility(bool value)
    {
        foreach (var item in altMesh.materials)
        {
            item.SetFloat("_isMainColor", value ? 0 : 1);
        }
    }
}
