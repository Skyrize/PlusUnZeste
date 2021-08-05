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
    // Transform cameraPivot;

    private void Start() {
        cameraTransform = Camera.main.transform;
        // cameraPivot = transform.Find("Pivot");
        // LookAt(transform.forward);
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

    List<Transform> obstructors = new List<Transform>();

    void CheckObstruction()
    {
        Vector3 direction = cameraTransform.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, direction.magnitude, mask);
        List<Transform> newObstructors = new List<Transform>();

        //Get new obstructors
        foreach (RaycastHit hit in hits)
        {
            newObstructors.Add(hit.transform);
            // if obstructor wasn't already there, goes shadowOnly
            if (obstructors.Remove(hit.transform) == false) {
                hit.transform.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }

        // Reset obstructors that don't obstruct anymore
        foreach (Transform obstructor in obstructors)
        {
            obstructor.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        obstructors = newObstructors;
    }

    public List<Material> mats = new List<Material>();
    void CheckObstructionShader()
    {
        Vector3 direction = cameraTransform.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, direction.magnitude, mask);
        List<Transform> newObstructors = new List<Transform>();

        //Get new obstructors
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.parent?.name == "Furnitures")
                continue;
            newObstructors.Add(hit.transform);
            // if obstructor wasn't already there, goes shadowOnly
            if (obstructors.Remove(hit.transform) == false) {
                mats.Clear();
                var renderers = hit.transform.GetComponentsInChildren<MeshRenderer>();
                
                foreach (var renderer in renderers)
                {
                    renderer.GetMaterials(mats);
                }
                foreach (var item in mats)
                {
                    item.SetFloat("_DissolvePercentage", 2);
                }
            }
        }

        // Reset obstructors that don't obstruct anymore
        foreach (Transform obstructor in obstructors)
        {
            mats.Clear();
            var renderers = obstructor.GetComponentsInChildren<MeshRenderer>();
            
            foreach (var renderer in renderers)
            {
                renderer.GetMaterials(mats);
            }
            foreach (var item in mats)
            {
                item.SetFloat("_DissolvePercentage", 0);
            }
        }
        obstructors = newObstructors;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        xInput += Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(0, xInput, 0);
        // CheckObstruction();
        CheckObstructionShader();
    } 
}
