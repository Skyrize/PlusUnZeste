using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SphereMovement movement = null;


    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Update is called once per frame
    void Update()
    {
        movement.direction.x = Input.GetAxis("Horizontal");
        movement.direction.z = Input.GetAxis("Vertical");
        movement.direction.Normalize();
        movement.direction = transform.forward * movement.direction.z + transform.right * movement.direction.x;
    }
}
