using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSaveManager : MonoBehaviour
{
    public InputSaveObject inputSave;
    public List<InputUI> inputUIS;
    // public Transform inputUIContainer;
    // public GameObject inputUIPrefab;

    static private InputSaveManager _instance = null;
    static public InputSaveManager instance {
        get {
            if (_instance == null)
                Debug.LogException(new System.Exception("Asking for instance too early (awake)"));
            return InputSaveManager._instance;
        }

        set {
            if (_instance) {
                Debug.LogException(new System.Exception("More thand one InputSaveManager in the Scene"));
            } else {
                _instance = value;
            }
        }
    }
    private void Awake() {
        instance = this;
        if (PlayerPrefs.GetInt("Jump", 0) == 0) {
            Debug.Log("First Save");
            inputSave.Save();
        }
        inputSave.Load();
    }

    public KeyCode GetKey(string inputName)
    {
        return inputSave.GetKey(inputName);
    }

    public void GenerateUI()
    {
        foreach (var item in inputUIS)
        {
            item.Generate();
        }
    }

    public void SetEnglishPreset()
    {
        inputSave.ChangeKey("Jump", KeyCode.Space);
        inputSave.ChangeKey("Forward", KeyCode.W);
        inputSave.ChangeKey("Backward", KeyCode.S);
        inputSave.ChangeKey("Left", KeyCode.A);
        inputSave.ChangeKey("Right", KeyCode.D);
        inputSave.ChangeKey("Restart", KeyCode.R);
        inputSave.ChangeKey("Pause", KeyCode.Escape);
        GenerateUI();
    }

    public void SetFrenchPreset()
    {
        inputSave.ChangeKey("Jump", KeyCode.Space);
        inputSave.ChangeKey("Forward", KeyCode.Z);
        inputSave.ChangeKey("Backward", KeyCode.S);
        inputSave.ChangeKey("Left", KeyCode.Q);
        inputSave.ChangeKey("Right", KeyCode.D);
        inputSave.ChangeKey("Restart", KeyCode.R);
        inputSave.ChangeKey("Pause", KeyCode.Escape);
        GenerateUI();
    }
 
}
