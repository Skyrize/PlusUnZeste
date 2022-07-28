using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JumpComponent : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float feetYOffset = .1f;
    [SerializeField] private float feetRadius = .3f;
    [SerializeField] private float sideOffset = .1f;
    [SerializeField] private float sideRadius = .3f;
    [SerializeField] private LayerMask groundMask;
    [HideInInspector] public UnityEvent onJump = new UnityEvent();
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform pivot;
    [Header("Runtime")]
    [SerializeField] private bool isGrounded = false;
    public bool IsGrounded => isGrounded;

    // bool isForceJump = false;
    // public void ForceJump()
    // {
    //     isForceJump = true;
    //     GetComponent<SphereMovement>().enabled = false;
    // }

    // Start is called before the first frame update
    void Start()
    {
        if (!rb)
            rb = GetComponent<Rigidbody>();
    }

    // private void OnCollisionEnter(Collision other) {
    //     bool isBellowY = other.GetContact(0).point.y - feetYOffset < transform.position.y;
    //     bool isBellowX = Mathf.Abs(other.GetContact(0).point.x - transform.position.x) < feetSideOffset;
    //     bool isBellowZ = Mathf.Abs(other.GetContact(0).point.z - transform.position.z) < feetSideOffset;
    //     bool isBellow = isBellowX && isBellowY && isBellowZ;
    //     if (other.gameObject.tag != "Trigger" && isBellow) {
    //         isGrounded = true;
    //         Debug.Log("hit" + other.GetContact(0).point.ToString());
    //         Debug.DrawRay(other.GetContact(0).point, Vector3.right * 10f, Color.red, 2);
    //         Debug.DrawRay(other.GetContact(0).point, -Vector3.right * 10f, Color.red, 2);
    //         Debug.DrawRay(other.GetContact(0).point, Vector3.forward * 10f, Color.red, 2);
    //         Debug.DrawRay(other.GetContact(0).point, -Vector3.forward * 10f, Color.red, 2);
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position - Vector3.up * feetYOffset, feetRadius, groundMask);

        if (!isGrounded) {
            bool isGroundLeft = Physics.CheckSphere(transform.position - transform.up * sideOffset, sideRadius, groundMask);
            bool isGroundRight = Physics.CheckSphere(transform.position + transform.up * sideOffset, sideRadius, groundMask);

            if (isGroundLeft && isGroundRight)
                isGrounded = true;
        }
        // if (isForceJump && isGrounded && Time.deltaTime != 0) {
        //     isForceJump = false;
        //     GetComponent<SphereMovement>().enabled = true;
        // }
        // if (jumpTimer <= 0) {
        //     canJump = true;
        // } else {
        //     jumpTimer -= Time.deltaTime;
        // }
        if (isGrounded && Input.GetKeyDown(InputSaveManager.instance.GetKey("Jump"))) {
            Jump(Vector3.up * jumpForce);
        }
    }

    public void Jump(Vector3 direction)
    {
        
        rb.AddForce(direction, ForceMode.Impulse);
        isGrounded = false;
        onJump.Invoke();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        // Gizmos.DrawWireCube(transform.position - Vector3.up * feetYOffset - Vector3.up * feetSideOffset / 2f, Vector3.one * feetSideOffset);
        Gizmos.DrawWireSphere(transform.position - Vector3.up * feetYOffset, feetRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.up * sideOffset, sideRadius);
        Gizmos.DrawWireSphere(transform.position - transform.up * sideOffset, sideRadius);
    }

}
