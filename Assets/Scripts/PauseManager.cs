using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject m_optionsUI;
    [SerializeField] GameObject m_pauseBackgroundUI;
    [HideInInspector] public UnityEvent onPause = new UnityEvent();
    [HideInInspector] public UnityEvent onUnpause = new UnityEvent(); 

    bool paused = false;

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        m_optionsUI.SetActive(true);
        m_pauseBackgroundUI.SetActive(true);
        onPause.Invoke();
    }

    public void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_optionsUI.SetActive(false);
        m_pauseBackgroundUI.SetActive(false);
        onUnpause.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(InputSaveManager.instance.GetKey("Pause"))) {
            if (paused) {
                Unpause();
            } else {
                Pause();
            }
        }
    }

    private void Start() {
        Unpause();
    }
}
