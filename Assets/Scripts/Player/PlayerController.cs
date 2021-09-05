﻿using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        //TODO : swith input
        // if (Input.GetKeyDown(KeyCode.F12))
        //     GetComponent<HealthComponent>().SetMaxHealth(1000000);
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
        //TODO : swith input
            // movement.direction.x = Input.GetAxis("Horizontal");
            // movement.direction.z = Input.GetAxis("Vertical");
            // movement.direction.Normalize();
            // movement.pivotInput = Input.GetAxis("Pivot");
        }
    }

    public void AskMove(InputAction.CallbackContext context)
    {

    }

    public void UpdateVisibility(bool value)
    {
        foreach (var item in altMesh.materials)
        {
            item.SetFloat("_isMainColor", value ? 0 : 1);
        }
    }
}
