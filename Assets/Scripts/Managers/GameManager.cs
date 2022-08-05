using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] string m_nextLevel = "ERROR_UNSET_NEXT_LEVEL";
    [SerializeField] string m_winAudioName = "Angels";
    [Space]
    [Header("Gameplay References")]
    [SerializeField] private PlayerMediator m_playerMediator;
    [SerializeField] private HealthComponent m_playerHealthComp;
    [SerializeField] private SceneManager m_sceneManager;
    [SerializeField] private AudioComponent m_audioComp;
    [SerializeField] private OnTouchEvent m_winZone;
    [SerializeField] private CheckpointManager m_checkpointManager;
    [Space]
    [Header("UI References")]
    [SerializeField] private UIFillBar m_playerHealthBar;
    [SerializeField] private UIFade m_winUIFade;
    [SerializeField] private UIFade m_respawnUIFade;

    void InitializeUIEvents()
    {
        m_playerHealthComp.onHealthRatioChanged.AddListener(m_playerHealthBar.SetFill);
    }

    void InitializeWinEvents()
    {
        m_winZone.onTouch.AddListener((GameObject _obj) => {
            m_audioComp.Play(m_winAudioName);
            m_winUIFade.Play();
            m_playerMediator.OnWin();
        });
        m_winUIFade.onFadeInEnd.AddListener(() => m_sceneManager.LoadScene(m_nextLevel));
    }

    void InitializeRespawnEvents()
    {
        m_playerHealthComp.onDeathEvent.AddListener(m_respawnUIFade.Play);
        m_respawnUIFade.onFadeInEnd.AddListener(() => {
            m_checkpointManager.Respawn();
            m_playerMediator.OnRespawn();
        });
    }

    private void Awake()
    {
        InitializeUIEvents();
        InitializeWinEvents();
        InitializeRespawnEvents();
    }

	public void Quit () 
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

    [SerializeField] private TMPro.TMP_Text timerUI; //TODO move in other class
    // Start is called before the first frame update
    void Start()
    {
        //TODO : move in save system
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
        
        if (Input.GetKeyDown(InputSaveManager.instance.GetKey("Respawn"))) {
            m_sceneManager.ReloadCurrentScene();
        }
    }
}
