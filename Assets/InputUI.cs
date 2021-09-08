using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class InputUI : MonoBehaviour
{
    public InputSaveObject saver;
    public TMPro.TMP_Text buttonText;
    public TMPro.TMP_Text inputText;
    public string inputName;

    bool isChanging = false;
    public void StartChange()
    {
        isChanging = true;
        buttonText.text = "Press key";
    }

    public void CancelChange()
    {
        isChanging = false;
        buttonText.text = saver.GetKey(inputName).ToString();

    }

    public void SetNewKey(KeyCode newKey)
    {
        saver.ChangeKey(inputName, newKey);
        Generate();
        isChanging = false;
    }

    private void Update() {
        if (isChanging && Input.anyKeyDown) {
            KeyCode key = KeyCode.Z;

            while (key != KeyCode.None && !Input.GetKeyDown(key)) {
                key -= 1;
            }
            if (key == KeyCode.None) {
                CancelChange();
            } else {
                SetNewKey(key);
            }
        }
    }

    private void Start() {
        Generate();
    }

    public void Generate()
    {
        if (!saver) {
            Debug.Log("No saver set");
            return;
        }
        inputText.text = inputName;

        buttonText.text = saver.GetKey(inputName).ToString();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InputUI))]
public class InputUIEditor : Editor
{
    public override void OnInspectorGUI () {
        DrawDefaultInspector();

        InputUI generator = target as InputUI;

        if(GUILayout.Button("(re)Generate")) {
            Undo.RecordObject(generator, "Generator Stove");
            generator.Generate();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
#endif
