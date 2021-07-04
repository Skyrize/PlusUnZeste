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

    public SubmergedEntity(Rigidbody rigidbody)
    {
        entityBody = rigidbody;
        this.baseDrag = rigidbody.drag;
        this.baseAngularDrag = rigidbody.angularDrag;
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

    [Header("References")]
    [SerializeField] private Transform waterLevel = null;

    [Header("Events")]
    [SerializeField] private WaterEvent onDive = new WaterEvent();
    [SerializeField] private WaterEvent onSwim = new WaterEvent();
    [SerializeField] private WaterEvent onGetOut = new WaterEvent();

    private List<SubmergedEntity> submergedEntities = new List<SubmergedEntity>();

    private void OnTriggerEnter(Collider other) {
        var properties = other.GetComponent<GameProperty>();
        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        
        if (properties && properties.HasProperty(GameProperty.Property.FLOAT) && otherBody != null && submergedEntities.Find(entity => entity.entityBody == otherBody) == null) {
            submergedEntities.Add(new SubmergedEntity(otherBody));
            otherBody.drag = waterDrag;
            otherBody.angularDrag = waterAngularDrag;
            onDive.Invoke(otherBody.transform);
        }
    }

    private void FixedUpdate() {
        foreach (SubmergedEntity entity in submergedEntities)
        {
            float displacement = waterLevel.position.y - entity.entityBody.transform.position.y;

            if (displacement > 0) {
                entity.entityBody.AddForce(Vector3.up * displacement * Buoyancy * 3, ForceMode.Acceleration);
                onSwim.Invoke(entity.entityBody.transform);
            }
        }
        
    }

    private void OnTriggerExit(Collider other) {
        var properties = other.GetComponent<GameProperty>();
        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        
        if (properties && properties.HasProperty(GameProperty.Property.FLOAT) && otherBody != null) {
            SubmergedEntity item = submergedEntities.Find((entity) => entity.entityBody == otherBody);

            if (item != null) {
                otherBody.drag = item.baseDrag;
                otherBody.angularDrag = item.baseAngularDrag;
                submergedEntities.Remove(item);
                onGetOut.Invoke(item.entityBody.transform);
            }
        }
    }
}
