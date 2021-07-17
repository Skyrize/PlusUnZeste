using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                paused = false;
                onUnpause.Invoke();
            } else {
                paused = true;
                onPause.Invoke();

            }
        }
    }
}
