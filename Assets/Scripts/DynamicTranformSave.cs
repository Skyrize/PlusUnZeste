using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct DynamicSave
{
    public Vector3 position;
    public Quaternion rotation;
    public Transform target;
}

[CreateAssetMenu(menuName = "DynamicTranformSave")]
public class DynamicTranformSave : ScriptableObject
{
    public DynamicSave[] saves;

#if UNITY_EDITOR
    public void Save()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length == 0) {
            Debug.LogError("Missing targets for save");
            return;
        }
        saves = new DynamicSave[targets.Length];
        for (int i = 0; i != targets.Length; i++) {
            var target = targets[i];
            saves[i].target = target.transform;
            saves[i].position = target.transform.position;
            saves[i].rotation = target.transform.rotation;
        }
    }

    public void Load()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length != saves.Length) {
            Debug.LogError("Data saved and targets selected mismatch");
            return;
        }
        for (int i = 0; i != targets.Length; i++) {
            var target = targets[i];
            var save = Array.Find(saves, (save) => save.target == target.transform);
            if (save.target == null) {
                Debug.LogError($"No save for transform {target.transform.name}");
                continue;
            }
            Undo.RecordObject(target.transform, "Load Dynamics");
            target.transform.position = save.position;
            target.transform.rotation = save.rotation;
        }
    }
#endif
}
