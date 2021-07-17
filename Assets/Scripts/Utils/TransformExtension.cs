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

    public static Transform GetDeepestChild(this Transform transform)
    {
        if (transform.childCount >= 1)
            return transform.GetChild(0).GetDeepestChild();
        return transform;
    }

    public static Transform[] GetAllChildren(this Transform transform)
    {
        Transform[] children = new Transform[transform.childCount];

        for (int i = 0; i != transform.childCount; i++) {
            children[i] = transform.GetChild(i);
        }
        return children;
    }

    public static Transform GetRandomChild(this Transform transform)
    {
        return transform.GetChild(Random.Range(0, transform.childCount));
    }

    public static T[] GetComponentsInDirectChildren<T>(this Transform transform, bool includeDisabled = false)
    {
        List<T> result = new List<T>();

        for (int i = 0; i != transform.childCount; i++) {
            var child = transform.GetChild(i);
            if (!includeDisabled && !child.gameObject.activeInHierarchy)
                continue;
            T comp = child.GetComponent<T>();
            if (comp != null)
                result.Add(comp);
        }
        return result.ToArray();
    }
}
