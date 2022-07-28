using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject m_player;
    [SerializeField] private UIFillBar m_playerHealthBar;
    [SerializeField] private CheckpointManager m_checkpointManager;
    [Header("Events")]
    [SerializeField] private UnityEvent onLevelStart = new UnityEvent();
    [SerializeField] private UnityEvent onWin = new UnityEvent();
    [SerializeField] private UnityEvent onLose = new UnityEvent();
    [SerializeField] private UnityEvent onRespawn = new UnityEvent();
    [SerializeField] private UnityEvent onRespawnCheckpoint = new UnityEvent();
    [SerializeField] private UnityEvent onRestart = new UnityEvent();

    void InitializePlayerEvents()
    {
        HealthComponent playerHealthComp = m_player.GetComponentInChildren<HealthComponent>();
        playerHealthComp.onDeathEvent.AddListener(Lose);
        playerHealthComp.onHealthRatioChanged.AddListener(m_playerHealthBar.SetFill);
    }

    private void Awake()
    {
        InitializePlayerEvents();
        m_checkpointManager.onRespawnKeyPressed.AddListener(Respawn);
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

    [SerializeField] private TMPro.TMP_Text timerUI; //TODO move in other class
    // Start is called before the first frame update
    void Start()
    {
        onLevelStart.Invoke();
        var savers = FindObjectsOfType<PlayerPrefBinder>(true);
        foreach (var item in savers)
        {
            item.Load();
        }

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
        timer += Time.deltaTime;
        PrintTimer();
    }
}
