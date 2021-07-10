using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform currentCheckpoint;
    Rigidbody rb;
    public Transform CurrentCheckpoint => currentCheckpoint;

    private void Awake() {
        currentCheckpoint = transform.Find("Start Checkpoint");
        if (!currentCheckpoint) {
            Debug.LogError("Need at least one checkpoint named \"Start Checkpoint\" for checkpoint Manager.");
        }
    }

    public void TriggerCheckpoint(Transform checkpoint)
    {
        Debug.Log($"new checkpoint {checkpoint.name}");
        currentCheckpoint = checkpoint;
    }

    public void Respawn()
    {
        player.position = currentCheckpoint.position;
    }
}
