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
    public Transform container;
    
#if UNITY_EDITOR
    // [MenuItem("Tools/Load %j")]
    public void LoadSelection()
    {
        if (!save) {
            Debug.LogError("Missing Save asset.");
            return;
        }
        save.Load();
    }
    public void LoadContainer()
    {
        if (!save) {
            Debug.LogError("Missing Save asset.");
            return;
        }
        save.Load(container);
    }

    // [MenuItem("Tools/Save %g")]
    public void SaveSelection()
    {
        if (!save) {
            Debug.LogError("Missing Save asset.");
            return;
        }
        save.Save();
    }

    public void SaveContainer()
    {
        if (!save) {
            Debug.LogError("Missing Save asset.");
            return;
        }
        if (!container) {
            Debug.LogError("Missing container.");
            return;
        }
        save.Save(container);
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

        if(GUILayout.Button("Save Selection")) {
            Undo.RecordObject(script, "Save Selected Dynamics");
            script.SaveSelection();
            // EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        if(GUILayout.Button("Save Container")) {
            Undo.RecordObject(script, "Save Contained Dynamics");
            script.SaveContainer();
            // EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        if(GUILayout.Button("Load Selection")) {
            Undo.RecordObject(script, "Load Selected Dynamics");
            script.LoadSelection();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        if(GUILayout.Button("Load Container")) {
            Undo.RecordObject(script, "Load Contained Dynamics");
            script.LoadContainer();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
#endif