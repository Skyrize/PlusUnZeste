using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KitchenStoveGenerator))]
public class KitchenStoveGeneratorEditor : Editor
{
    public override void OnInspectorGUI () {
        DrawDefaultInspector();

        KitchenStoveGenerator generator = target as KitchenStoveGenerator;

        if(GUILayout.Button("(re)Generate")) {
            generator.Generate();
        }
    }
}
