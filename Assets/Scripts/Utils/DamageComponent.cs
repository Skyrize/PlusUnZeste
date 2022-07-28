using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageComponent : MonoBehaviour
{
    [Header("Damage on touch")]
    [SerializeField] private float m_damageAmount = 1;
    [SerializeField] private float m_cooldown = 0.75f;
    private bool m_isOnCooldown = false;

    [Header("Damage over time")]
    [SerializeField] private float m_damagePerTick = 1;
    [SerializeField] private float m_tickPerSecond = 5;

    [HideInInspector] public UnityEvent<GameObject, float> onDamage = new UnityEvent<GameObject, float>();

    Dictionary<HealthComponent, Coroutine> m_targetsOfOverTimeDamages = new Dictionary<HealthComponent, Coroutine>();

#if !UNITY_EDITOR
    WaitForSeconds m_tickTimer;
    WaitForSeconds m_cooldownTimer;

    private void Awake()
    {
        m_tickTimer = new WaitForSeconds(1.0f / m_tickPerSecond);
        m_cooldownTimer = new WaitForSeconds(m_cooldown);
    }
#endif
    
    private void OnTriggerEnter(Collider other) {
        HealthComponent otherHealth = other.GetComponent<HealthComponent>();

        if (otherHealth && !m_targetsOfOverTimeDamages.ContainsKey(otherHealth))
        {
            m_targetsOfOverTimeDamages.Add(otherHealth, StartCoroutine(DamageOverTime(otherHealth)));
        }
    }

    private void OnTriggerExit(Collider other) {
        HealthComponent otherHealth = other.GetComponent<HealthComponent>();
        Coroutine coroutine;

        if (otherHealth && m_targetsOfOverTimeDamages.Remove(otherHealth, out coroutine))
        {
            StopCoroutine(coroutine);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contactPoint = other.GetContact(0);
        HealthComponent otherHealth = other.gameObject.GetComponent<HealthComponent>();

        if (otherHealth && !m_isOnCooldown) {
            StartCoroutine(DamageOnTouch(otherHealth));
            Damage(otherHealth);
        }
    }

    IEnumerator DamageOverTime(HealthComponent _target)
    {
        while (true)
        {
            Damage(_target);
#if UNITY_EDITOR
            yield return new WaitForSeconds(1.0f / m_tickPerSecond);
#else
            yield return m_tickTimer;
#endif
        }
    }

    IEnumerator DamageOnTouch(HealthComponent _target)
    {
        Damage(_target);
        m_isOnCooldown = true;
#if UNITY_EDITOR
        yield return new WaitForSeconds(m_cooldown);
#else
        yield return m_cooldownTimer;
#endif
        m_isOnCooldown = false;
    }

    void Damage(HealthComponent _target)
    {
        _target.ReduceHealth(m_damageAmount);
        onDamage.Invoke(_target.gameObject, m_damageAmount);
    }
}
