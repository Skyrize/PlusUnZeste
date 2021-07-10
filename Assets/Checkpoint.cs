using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager manager;

    private void Awake() {
        manager = transform.parent.GetComponent<CheckpointManager>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && manager.CurrentCheckpoint != transform) {
            manager.TriggerCheckpoint(transform);
        }
    }
}
