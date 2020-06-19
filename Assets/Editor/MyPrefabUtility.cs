using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyPrefabUtility
{
    [MenuItem ("MyTools/Create Prefabs from selection")]
    static void CreatePrefabsFromSelection()
    {
        foreach (GameObject gameObject in Selection.gameObjects) {
            string localPath = "Assets/Prefabs/" + gameObject.name + ".prefab";

            // Make sure the file name is unique, in case an existing Prefab has the same name.
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            // Create the new Prefab.
            PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
        }
    }

    // Disable the menu item if no selection is in place.
    [MenuItem("MyTools/Create Prefabs from selection", true)]
    static bool ValidateCreatePrefab()
    {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject);
    }
}
