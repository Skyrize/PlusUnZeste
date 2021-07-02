using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension {
    public static void DestroyChilds(this Transform transform) {
        while (transform.childCount != 0) {
            if (Application.isPlaying) {
                GameObject.Destroy(transform.GetChild(0).gameObject);
            } else {
                GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}
