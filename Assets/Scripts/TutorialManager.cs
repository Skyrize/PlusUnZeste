using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor;

[System.Serializable]
public struct TutorialStep {
    [TextArea]
    public string text;
    public UnityEvent onStepBegin;
    public UnityEvent onStepEnd;
}

public class TutorialManager : MonoBehaviour
{
    [Header("Steps")]
    [SerializeField]
    private TutorialStep[] steps = null;
    [Header("References")]
    [SerializeField] private Text tutorialTextDisplay = null;
    private int currentStep = 0;

    private void OnGUI() {
        GUIStyle style = new GUIStyle ();
        style.richText = true;
    }

    public void StartStep(int index)
    {
        steps[index].onStepBegin.Invoke();
        tutorialTextDisplay.text = steps[index].text;
        currentStep = index;
    }

    public void EndCurrentStep()
    {
        steps[currentStep].onStepEnd.Invoke();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartStep(0);
    }

    private void Update() {
        //TODO : swith input
        // if (tutorialTextDisplay.isActiveAndEnabled && Input.GetKeyDown(KeyCode.Space)) {
        //     EndCurrentStep();
        // }
    }
}
