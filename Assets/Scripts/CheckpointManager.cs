using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform currentCheckpoint;
    [SerializeField] float playerHealthSave = 0;
    Rigidbody rb;
    public Transform CurrentCheckpoint => currentCheckpoint;
    public UnityEvent onTrigger = new UnityEvent();

    private void Awake() {
        currentCheckpoint = transform.Find("Start Checkpoint");
        if (!currentCheckpoint) {
            Debug.LogError("Need at least one checkpoint named \"Start Checkpoint\" for checkpoint Manager.");
        }
        playerHealthSave = player.GetComponent<HealthComponent>().Health;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            GameManager.instance.Lose();
        }
    }

    public void TriggerCheckpoint(Transform checkpoint)
    {
        Debug.Log($"new checkpoint {checkpoint.name}");
            //TODO : save state of game by retrieving "saveComponents" which capture value of each dynamics objects and allow to reset them
        playerHealthSave = player.GetComponent<HealthComponent>().Health;
        onTrigger.Invoke();
        currentCheckpoint = checkpoint;
    }

    public void Respawn()
    {
        player.position = currentCheckpoint.position;
        player.GetComponent<HealthComponent>().SetHealth(playerHealthSave);
    }
}
