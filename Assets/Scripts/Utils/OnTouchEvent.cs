using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollisionEvent : UnityEvent<GameObject>
{
}

public class OnTouchEvent : MonoBehaviour
{
    [SerializeField] private CollisionEvent onTouch = new CollisionEvent();

    private void OnCollisionEnter(Collision other) {
        onTouch.Invoke(other.gameObject);
    }
    private void OnTriggerEnter(Collider other) {
        onTouch.Invoke(other.gameObject);
    }
}
