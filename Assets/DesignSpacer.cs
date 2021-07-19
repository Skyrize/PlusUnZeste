using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

public class DesignSpacer : MonoBehaviour
{
    [SerializeField] float spacing = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #if UNITY_EDITOR
    public void Place()
    {
        for (int i = 0; i != transform.childCount; i++) {
            transform.GetChild(i).position = transform.position + Vector3.right * spacing * i;
        }
    }
    #endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(DesignSpacer))]
public class DesignSpacerEditor : Editor
{
    public override void OnInspectorGUI () {
        DrawDefaultInspector();

        DesignSpacer script = target as DesignSpacer;

        if(GUILayout.Button("Place")) {
            script.Place();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
#endif