using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public UnityEvent onTrigger = new UnityEvent();
    CheckpointManager manager;

    private void Awake() {
        manager = transform.parent.GetComponent<CheckpointManager>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && manager.CurrentCheckpoint != transform) {
            onTrigger.Invoke();
            manager.TriggerCheckpoint(transform);
        }
    }
}
