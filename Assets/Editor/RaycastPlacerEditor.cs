using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(RaycastPlacer))]
public class RaycastPlacerEditor : Editor
{
    public override void OnInspectorGUI () {
        DrawDefaultInspector();

        RaycastPlacer script = target as RaycastPlacer;

        if(GUILayout.Button("(re)Place")) {
            Undo.RecordObject(script, "Raycast Placing");
            script.Replace();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
