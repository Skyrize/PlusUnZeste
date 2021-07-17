using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class DynamicGenerator : MonoBehaviour
{
    public Transform staticContainer;
    public Transform dynamicContainer;
    public Transform endContainer;

    public void Generate()
    {
        Transform[] statics = staticContainer.GetAllChildren();
        Transform[] dynamics = dynamicContainer.GetAllChildren();

        foreach (var item in dynamics)
        {
        }
        foreach (var item in statics)
        {
            Debug.Log(item.name);
            if (item.name.Contains("Variant") || item.name.Contains("dynamic")) {
            Debug.Log("ejected");
                continue;
            }
            if (Array.Find<Transform>(dynamics, (dyn) => dyn.name == item.name) == null) {
                item.parent = endContainer;
                item.localPosition = Vector3.zero;
                item.gameObject.AddComponent<Rigidbody>();
                var colliders = item.GetComponentsInChildren<MeshCollider>(true);
                foreach (var col in colliders)
                {
                    col.convex = true;
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DynamicGenerator))]
public class DynamicGeneratorEditor : Editor
{
    public override void OnInspectorGUI () {
        DrawDefaultInspector();

        DynamicGenerator script = target as DynamicGenerator;

        if(GUILayout.Button("Generate")) {
            Undo.RecordObject(script, "Generate Dynamics");
            script.Generate();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
#endif