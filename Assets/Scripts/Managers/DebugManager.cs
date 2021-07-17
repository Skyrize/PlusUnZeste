using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject debugArrowPrefab = null;
    [SerializeField] private SceneManager sceneManager = null;
    static private DebugManager _instance = null;
    static public DebugManager instance {
        get {
            if (_instance == null)
                Debug.LogException(new System.Exception("Asking for instance too early (awake)"));
            return DebugManager._instance;
        }

        set {
            if (_instance) {
                Debug.LogException(new System.Exception("More thand one DebugManager in the Scene"));
            } else {
                _instance = value;
            }
        }
    }
    private void Awake() {
        instance = this;
        // DontDestroyOnLoad(this.gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            sceneManager.LoadScene("Level 1");
        }
        if (Input.GetKeyDown(KeyCode.F2)) {
            sceneManager.LoadScene("Level 2");
        }
        if (Input.GetKeyDown(KeyCode.F3)) {
            sceneManager.LoadScene("Level 3");
        }
        if (Input.GetKeyDown(KeyCode.F4)) {
            sceneManager.LoadScene("Level 4");
        }
        if (Input.GetKeyDown(KeyCode.F5)) {
            sceneManager.LoadScene("Level 5");
        }
        if (Input.GetKeyDown(KeyCode.F6)) {
            sceneManager.LoadScene("Level 6");
        }
    }

    public void CreateDebugArrow(ContactPoint contactPoint, bool reverse = false)
    {
        if (reverse) {
            CreateDebugArrow(contactPoint.point, contactPoint.point - contactPoint.normal);
        } else {
            CreateDebugArrow(contactPoint.point, contactPoint.point + contactPoint.normal);
        }
    }

    public void CreateDebugArrow(Vector3 position, Vector3 lookAt)
    {
        var arrow = GameObject.Instantiate(debugArrowPrefab, position, Quaternion.identity, transform);

        arrow.transform.LookAt(lookAt);
    }

    public void Log(string message)
    {
        Debug.Log(message);
    }

}
