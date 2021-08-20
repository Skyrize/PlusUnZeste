using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMPro.TMP_Text timerUI;
    [Header("Events")]
    [SerializeField] private UnityEvent onLevelStart = new UnityEvent();
    [SerializeField] private UnityEvent onWin = new UnityEvent();
    [SerializeField] private UnityEvent onLose = new UnityEvent();
    [SerializeField] private UnityEvent onRespawn = new UnityEvent();
    [SerializeField] private UnityEvent onRespawnCheckpoint = new UnityEvent();
    [SerializeField] private UnityEvent onRestart = new UnityEvent();

    GameObject cook;

    static private GameManager _instance = null;
    static public GameManager instance {
        get {
            if (_instance == null)
                Debug.LogException(new System.Exception("Asking for instance too early (awake)"));
            return GameManager._instance;
        }

        set {
            if (_instance) {
                Debug.LogException(new System.Exception("More thand one GameManager in the Scene"));
            } else {
                _instance = value;
            }
        }
    }
    private void Awake() {
        instance = this;
        cook = FindObjectOfType<CookerController>(true)?.gameObject;
        var savers = FindObjectsOfType<PlayerPrefBinder>(true);
        foreach (var item in savers)
        {
            item.Load();
        }
        // DontDestroyOnLoad(this.gameObject);
    }

    public void DisableMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void FreezeTime()
    {
        Time.timeScale = 0;
    }

    public void EnableMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

	public void Quit () 
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

    public void UnfreezeTime()
    {
        Time.timeScale = 1;
    }

    public void Win()
    {
        onWin.Invoke();
    }
    public void Lose()
    {
        onLose.Invoke();
    }

    public void Respawn()
    {
        onRespawn.Invoke();
    }

    public void RespawnCheckpoint()
    {
        onRespawnCheckpoint.Invoke();
    }

    public void Restart()
    {
        onRestart.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        onLevelStart.Invoke();
        timerUI.text = "00:00";
    }

    float timer = 0;

    void PrintTimer()
    {
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        timerUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (cook && Input.GetKey(KeyCode.B))
            if (Input.GetKeyDown(KeyCode.N))
                cook.SetActive(!cook.activeInHierarchy);
        timer += Time.deltaTime;
        PrintTimer();
    }
}
