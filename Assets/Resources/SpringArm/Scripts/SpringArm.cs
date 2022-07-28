using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpringArm : MonoBehaviour
{
    [Space]
    [Header("HAS TO BE CHILD \n--------------------")]
    [Space]
    public Transform m_target;

    [Space]
    [Header("Collision Settings \n-----------------------")]
    [Space]
    public float m_collisionCastSize = 0.3f;
    public float m_collisionSmoothTime = 0.05f;
    public LayerMask m_collisionLayerMask = ~0;
    public float m_upRayOffset = 0.3f;

    [Space]
    [Header("Debugging \n--------------")]
    [Space]
    public bool m_visualDebugging = true;

    #region Private Variables
    
    private Vector3 m_direction;
    private Vector3 m_targetBasePos;
    private float m_targetArmLength = 3f;
    private RaycastHit m_hit;
    private Vector3 m_targetPosition;

    // refs for SmoothDamping
    private Vector3 m_moveVelocity;
    private Vector3 m_collisionTestVelocity;

    #endregion

    void Initialize()
    {
        m_targetBasePos = m_target.localPosition;
        m_direction = Vector3.up * m_upRayOffset + m_targetBasePos;
        m_targetArmLength = m_direction.magnitude;
        m_direction.Normalize();
    }

    private void Start()
    {
        if (m_target.parent != transform)
            throw new UnityException("Target has to be a child");
        Initialize();
    }

    private void OnValidate()
    {
        Initialize();
    }
    bool collided = false;

    private void Update()
    {
        Vector3 direction = transform.TransformDirection(m_direction);
        collided = Physics.SphereCast(transform.position, m_collisionCastSize, direction, out m_hit, m_targetArmLength, m_collisionLayerMask);
        if (collided)
        {
            Debug.Log("Collided " + m_hit.transform.gameObject.name);
            m_targetPosition = m_direction * m_hit.distance;
        }
        else
        {
            m_targetPosition = m_targetBasePos;
        }
    }

    private void LateUpdate() {
        ProcessSpring();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!m_visualDebugging)
            return;
        Vector3 startPos = transform.position + Vector3.up * m_upRayOffset;
        if (collided)
        {
            Gizmos.color = Color.red;
            Vector3 targetWorldPos = transform.TransformPoint(m_targetPosition);
            Gizmos.DrawWireSphere(targetWorldPos, m_collisionCastSize);
            Gizmos.DrawLine(startPos, targetWorldPos);
        }
        else
        {
            Gizmos.color = Color.green;
            Vector3 targetBaseWorldPos = transform.TransformPoint(m_targetBasePos);
            Gizmos.DrawWireSphere(targetBaseWorldPos, m_collisionCastSize);
            Gizmos.DrawLine(startPos, targetBaseWorldPos);
        }
    }
#endif
    
    private void ProcessSpring()
    {
        m_target.localPosition = Vector3.SmoothDamp(m_target.localPosition, m_targetPosition, ref m_collisionTestVelocity, m_collisionSmoothTime);
    }
}
