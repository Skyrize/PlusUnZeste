using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField]
    private float _actualHealth = 100;
    [SerializeField]
    private float maxHealth = 100;

    [Header("Events")]
    [HideInInspector] public UnityEvent<float> onHealEvent = new UnityEvent<float>();
    [HideInInspector] public UnityEvent<float> onDamageEvent = new UnityEvent<float>();
    [HideInInspector] public UnityEvent onDeathEvent = new UnityEvent();
    [HideInInspector] public UnityEvent<float> onHealthRatioChanged = new UnityEvent<float>();

    private float actualHealth {
        get {
            return _actualHealth;
        }
        set {
            _actualHealth = Mathf.Clamp(value, 0, maxHealth);
            onHealthRatioChanged.Invoke(_actualHealth / maxHealth);
        }
    }

    public float HealthRatio => _actualHealth / maxHealth;
    public float Health => _actualHealth;
    public float MaxHealth => maxHealth;
    public bool IsAlive => actualHealth > 0;
    public bool IsDead => !IsAlive;
    public bool IsFullHealth => actualHealth == maxHealth;

    public void SetHealth(float health)
    {
        actualHealth = health;
    }

    public void ReduceHealth(float amount)
    {
        if (IsDead)
            return;
        actualHealth -= amount;
        onDamageEvent.Invoke(amount);
        if (IsDead)
            onDeathEvent.Invoke();
    }
    
    public void IncreaseHealth(float amount)
    {
        if (IsDead)
            return;
        actualHealth += amount;
        onHealEvent.Invoke(amount);
    }

    public void Kill() {
        Destroy(this.gameObject);
    }

    public void SetMaxHealth(float newMax)
    {
        this.maxHealth = newMax;
        this.actualHealth = newMax;
    }

    public void FullHealth()
    {
        actualHealth = maxHealth;
    }

}
