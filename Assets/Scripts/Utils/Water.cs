using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class WaterEvent : UnityEvent<Transform>
{
}

public class SubmergedEntity {
    public Rigidbody entityBody;
    public float baseDrag;
    public float baseAngularDrag;
    public GameObject ripple;

    public SubmergedEntity(Rigidbody rigidbody, GameObject ripplePrefab)
    {
        entityBody = rigidbody;
        this.baseDrag = rigidbody.drag;
        this.baseAngularDrag = rigidbody.angularDrag;
        this.ripple = GameObject.Instantiate(ripplePrefab);
        if (entityBody.transform.tag == "Player") {
            var ps = this.ripple.GetComponent<ParticleSystem>();
            var main = ps.main;
            
            main.startDelay = 0f;
        }
    }
    
    // public override bool Equals(object obj)
    // {
    //     return base.Equals(obj);
    // }
    
    // public override int GetHashCode()
    // {
    //     return base.GetHashCode();
    // }
    
    // public static bool operator ==(SubmergedEntity f1, SubmergedEntity f2) { return f1.entityBody == f2.entityBody && f1.baseDrag == f2.baseDrag && f1.baseAngularDrag == f2.baseAngularDrag; }
    // public static bool operator !=(SubmergedEntity f1, SubmergedEntity f2) { return !(f1==f2); }
}

public class Water : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float Buoyancy = 1;
    [SerializeField] private float waterDrag = 1;
    [SerializeField] private float waterAngularDrag = 1;
    [SerializeField] private float splashFactor = 1;

    [Header("References")]
    [SerializeField] private Transform waterLevel = null;
    [SerializeField] private GameObject ripplePrefab = null;
    [SerializeField] private GameObject splashPrefab = null;

    [Header("Events")]
    [SerializeField] private WaterEvent onDive = new WaterEvent();
    [SerializeField] private WaterEvent onSwim = new WaterEvent();
    [SerializeField] private WaterEvent onGetOut = new WaterEvent();

    private List<SubmergedEntity> submergedEntities = new List<SubmergedEntity>();

    private void OnTriggerEnter(Collider other) {
        var properties = other.GetComponent<GameProperty>();
        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        
        if (properties && properties.Floatable && otherBody != null && submergedEntities.Find(entity => entity.entityBody == otherBody) == null) {
            submergedEntities.Add(new SubmergedEntity(otherBody, ripplePrefab));
            otherBody.drag = waterDrag;
            otherBody.angularDrag = waterAngularDrag;
            var ps = GameObject.Instantiate(splashPrefab, new Vector3(otherBody.transform.position.x, waterLevel.transform.position.y, otherBody.transform.position.z), Quaternion.identity).GetComponent<CartoonFX.CFXR_Effect>();
            ps.ScaleParticle(otherBody.velocity.magnitude * splashFactor);
            onDive.Invoke(otherBody.transform);
        }
    }
    public float floatingForce = 1;

    private void FixedUpdate() {
        foreach (SubmergedEntity entity in submergedEntities)
        {
            float displacement = waterLevel.position.y - entity.entityBody.transform.position.y + Mathf.Sin(Time.time + entity.entityBody.transform.position.x + entity.entityBody.transform.position.z) * floatingForce;

            if (displacement > 0) {
                entity.entityBody.AddForce(Vector3.up * displacement * Buoyancy * 3, ForceMode.Acceleration);
                entity.ripple.transform.position = new Vector3(entity.entityBody.transform.position.x, waterLevel.transform.position.y, entity.entityBody.transform.position.z);
                onSwim.Invoke(entity.entityBody.transform);
            }
        }
        
    }

    private void OnTriggerExit(Collider other) {
        var properties = other.GetComponent<GameProperty>();
        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        
        if (properties && properties.Floatable && otherBody != null) {
            SubmergedEntity item = submergedEntities.Find((entity) => entity.entityBody == otherBody);

            if (item != null) {
                otherBody.drag = item.baseDrag;
                otherBody.angularDrag = item.baseAngularDrag;
                var ps = item.ripple.GetComponent<ParticleSystem>();
                var main = ps.main;
                main.loop = false;
                submergedEntities.Remove(item);
                onGetOut.Invoke(item.entityBody.transform);
                item = null;
            }
        }
    }
}
