using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleBinder : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.localScale != transform.parent.localScale)
            transform.localScale = transform.parent.localScale;
    }
}
