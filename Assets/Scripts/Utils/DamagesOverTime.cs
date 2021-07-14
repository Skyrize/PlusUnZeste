using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class DamageOtherEvent : UnityEvent<Transform>
{
}

public class DamagesOverTime : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damagePerTick = 1;
    [SerializeField] private float TickPerSecond = 5;
    public DamageOtherEvent onDamage = new DamageOtherEvent();

    private HealthComponent target = null;
    private float timer = 0;
    
    private void OnTriggerEnter(Collider other) {
        target = other.GetComponent<HealthComponent>();
    }

    private void OnTriggerExit(Collider other) {
        if (target && other.gameObject == target.gameObject)
            target = null;
    }

    private void Update() {
        if (target) {
            if (timer <= 0) {
                timer = 1.0f / TickPerSecond;
                target.ReduceHealth(damagePerTick);
                onDamage.Invoke(target.transform);
            } else {
                timer -= Time.deltaTime;
            }
        }
    }
}
