using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagesOnTouch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damageAmount = 1;
    [SerializeField] private float cooldown = 0.75f;
    [SerializeField] private bool isCooldown = true;
    [Header("References")]
    [SerializeField]
    private List<Collider> hitBoxes;
    public UnityEvent<GameObject, float> onTouch = new UnityEvent<GameObject, float>();

    IEnumerator Cooldown()
    {
        isCooldown = false;
        yield return new WaitForSeconds(cooldown);
        isCooldown = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contactPoint = other.GetContact(0);
        HealthComponent otherHealth = other.gameObject.GetComponent<HealthComponent>();

        if (otherHealth != null && hitBoxes.Contains(contactPoint.thisCollider) && isCooldown == true) {
            if (cooldown != 0.0f)
                StartCoroutine(Cooldown());
            otherHealth.ReduceHealth(damageAmount);
            onTouch.Invoke(other.gameObject, damageAmount);
        }
        
    }

    private void Awake()
    {
        if (hitBoxes.Count == 0)
            hitBoxes.AddRange(GetComponentsInChildren<Collider>());
    }
}
