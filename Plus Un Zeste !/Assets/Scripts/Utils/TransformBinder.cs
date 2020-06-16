﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBinder : MonoBehaviour
{
    [Header("References")]
    public Transform target = null;
    private Vector3 distance = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        distance = transform.position - target.position;
    }

    private void LateUpdate() {
        transform.position = target.position + distance;
    }
}