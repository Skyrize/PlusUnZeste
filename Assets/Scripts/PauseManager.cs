using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public UnityEvent onPause = new UnityEvent();
    public UnityEvent onUnpause = new UnityEvent(); 

    public bool paused = false;


    public void Pause()
    {
        onPause.Invoke();
        paused = true;
    }

    public void Unpause()
    {
        onUnpause.Invoke();
        paused = false;
    }
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
            Pause();
    }

    public void Unpause(InputAction.CallbackContext context)
    {
        if (context.performed)
            Unpause();
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (context.performed) {
            if (paused) {
                Unpause();
            } else {
                Pause();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO : swith input
        // if (Input.GetKeyDown(InputSaveManager.instance.GetKey("Pause"))) {
        //     if (paused) {
        //         paused = false;
        //         onUnpause.Invoke();
        //     } else {
        //         paused = true;
        //         onPause.Invoke();

        //     }
        // }
    }

    private void Start() {
        Unpause();
    }
}
