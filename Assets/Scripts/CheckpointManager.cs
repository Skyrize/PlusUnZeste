using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform currentCheckpoint;
    [SerializeField] Transform cook;
    [SerializeField] float playerHealthSave = 0;
    Rigidbody rb;
    public Transform CurrentCheckpoint => currentCheckpoint;
    public UnityEvent onTrigger = new UnityEvent();
    [HideInInspector] public UnityEvent onRespawnKeyPressed = new UnityEvent();
    CameraController cam;
    Vector3 baseInertia;

//TODO : rework
    private void Awake() {
        currentCheckpoint = transform.Find("Start Checkpoint");
        if (!currentCheckpoint) {
            Debug.LogError("Need at least one checkpoint named \"Start Checkpoint\" for checkpoint Manager.");
        }
        playerHealthSave = player.GetComponent<HealthComponent>().Health;
        cam = player.parent.Find("Camera, Texts & Sound").GetComponent<CameraController>();
        baseInertia = player.GetComponent<Rigidbody>().inertiaTensor;
    }

    private void Update() {
        if (Input.GetKeyDown(InputSaveManager.instance.GetKey("Respawn"))) {
            onRespawnKeyPressed.Invoke();
        }

        if (Input.GetKey(KeyCode.C)) {
            for (int i = 0; i != 10; i++) {
                if (Input.GetKeyDown(KeyCode.Keypad0 + i) && transform.childCount > i) {
                    TriggerCheckpoint(transform.GetChild(i));
                    Respawn();
                }
            }
        }
    }

    public void TriggerCheckpoint(Transform checkpoint)
    {
        // Debug.Log($"new checkpoint {checkpoint.name}");
            //TODO : save state of game by retrieving "saveComponents" which capture value of each dynamics objects and allow to reset them
        playerHealthSave = player.GetComponent<HealthComponent>().Health;
        onTrigger.Invoke();
        currentCheckpoint = checkpoint;
        cook.GetComponent<CookerController>().SaveState();
        cook.GetComponent<SeekTarget>().SaveState();
    }

    public void Respawn()
    {
        cam.LookAt(player.position + currentCheckpoint.forward);
        player.position = currentCheckpoint.position;
        player.transform.forward = currentCheckpoint.forward;
        player.transform.up = -currentCheckpoint.right;
        player.GetComponent<HealthComponent>().SetHealth(playerHealthSave);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().inertiaTensor = baseInertia;
        player.GetComponent<Rigidbody>().inertiaTensorRotation = Quaternion.identity;
        cook.GetComponent<CookerController>().LoadState();
        cook.GetComponent<SeekTarget>().LoadState();
    }
}
