using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SphereMovement movement = null;
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
            movement.direction.x = Input.GetAxis("Horizontal");
            movement.direction.z = Input.GetAxis("Vertical");
            movement.direction.Normalize();
            movement.pivotInput = Input.GetAxis("Pivot");
        }
    }
}
