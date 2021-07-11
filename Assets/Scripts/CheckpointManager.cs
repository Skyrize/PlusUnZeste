using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform currentCheckpoint;
    Rigidbody rb;
    public Transform CurrentCheckpoint => currentCheckpoint;
    public UnityEvent onTrigger = new UnityEvent();

    private void Awake() {
        currentCheckpoint = transform.Find("Start Checkpoint");
        if (!currentCheckpoint) {
            Debug.LogError("Need at least one checkpoint named \"Start Checkpoint\" for checkpoint Manager.");
        }
    }

    public void TriggerCheckpoint(Transform checkpoint)
    {
        Debug.Log($"new checkpoint {checkpoint.name}");
            //TODO : save state of game by retrieving "saveComponents" which capture value of each dynamics objects and allow to reset them
        onTrigger.Invoke();
        currentCheckpoint = checkpoint;
    }

    public void Respawn()
    {
        player.position = currentCheckpoint.position;
    }
}
