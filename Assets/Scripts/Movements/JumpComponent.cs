using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpComponent : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce = 10;
    public UnityEvent onJump = new UnityEvent();
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [Header("Runtime")]
    [SerializeField] private bool jump = false;
    [SerializeField] private bool CanJump = false;



    // Start is called before the first frame update
    void Start()
    {
        if (!rb)
            rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag != "Trigger" && other.GetContact(0).point.y < transform.position.y && other.gameObject.tag == "Walkable")
            CanJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Jump") != 0 && CanJump) {
            jump = true;
        }
    }

    private void LateUpdate() {
        if (jump && CanJump) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            CanJump = false;    
            jump = false;
            onJump.Invoke();
        }
    }

}
