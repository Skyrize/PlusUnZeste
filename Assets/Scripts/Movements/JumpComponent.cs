using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpComponent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float jumpDelay = .1f;
    [SerializeField] private UnityEvent onJump = new UnityEvent();
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [Header("Runtime")]
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool canJump = true;
    [SerializeField] private float jumpTimer = 0f;



    // Start is called before the first frame update
    void Start()
    {
        if (!rb)
            rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag != "Trigger" && other.GetContact(0).point.y < transform.position.y && other.gameObject.tag == "Walkable")
            isGrounded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpTimer <= 0) {
            canJump = true;
        } else {
            jumpTimer -= Time.deltaTime;
        }
        if (canJump && Input.GetButtonDown(Keycode.space)) {
            Jump(Vector3.up * jumpForce);
        }
    }

    public void Jump(Vector3 direction)
    {
        
        rb.AddForce(direction, ForceMode.Impulse);
        isGrounded = false;
        canJump = false;
        jumpTimer = jumpDelay;
        onJump.Invoke();
    }

}
