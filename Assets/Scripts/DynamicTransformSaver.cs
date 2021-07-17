using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class DynamicTransformSaver : MonoBehaviour
{
    public DynamicTranformSave save;
    
#if UNITY_EDITOR
    // [MenuItem("Tools/Load %j")]
    public void Load()
    {
        if (!save) {
            Debug.LogError("Missing Save asset.");
            return;
        }
        save.Load();
    }

    // [MenuItem("Tools/Save %g")]
    public void Save()
    {
        if (!save) {
            Debug.LogError("Missing Save asset.");
            return;
        }
        save.Save();
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(DynamicTransformSaver))]
public class DynamicTransformSaverEditor : Editor
{
    public override void OnInspectorGUI () {
        DrawDefaultInspector();

        DynamicTransformSaver script = target as DynamicTransformSaver;

        if(GUILayout.Button("Save")) {
            Undo.RecordObject(script, "Save Dynamics");
            script.Save();
            // EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        if(GUILayout.Button("Load")) {
            Undo.RecordObject(script, "Load Dynamics");
            script.Load();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
#endif