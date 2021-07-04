using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(KitchenStoveGenerator))]
public class KitchenStoveGeneratorEditor : Editor
{
    public override void OnInspectorGUI () {
        DrawDefaultInspector();

        KitchenStoveGenerator generator = target as KitchenStoveGenerator;

        if(GUILayout.Button("(re)Generate")) {
            Undo.RecordObject(generator, "Generator Stove");
            generator.Generate();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
