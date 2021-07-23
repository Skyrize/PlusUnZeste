using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToggleButtonUI : MonoBehaviour
{
    public bool state = false;
    public string trueStateText = "Hide Timer";
    public string falseStateText = "Show Timer";
    public TMPro.TMP_Text text;

    public UnityEvent onToggleTrue = new UnityEvent();
    public UnityEvent onToggleFalse = new UnityEvent();
    

    public void Toggle()
    {
        state = !state;
        
        if (state) {
            text.text = trueStateText;
            onToggleTrue.Invoke();
        } else {
            text.text = falseStateText;
            onToggleFalse.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        state = !state;
        Toggle();
    }

}
