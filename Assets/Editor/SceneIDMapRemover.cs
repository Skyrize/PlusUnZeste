using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneIDMapRemover : Editor
{
    [MenuItem("Tools/Remove SceneIDMap %h")]
    public static void Remove() {
        
        Selection.activeGameObject = GameObject.Find("SceneIDMap");
        DestroyImmediate(Selection.activeGameObject);
        Selection.activeGameObject = GameObject.Find("StaticLightingSky");
        DestroyImmediate(Selection.activeGameObject);
    }
}
