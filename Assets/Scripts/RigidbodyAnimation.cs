using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RigidbodyAnimation : MonoBehaviour
{
    public float duration;
    public float endValue = 100;
    Rigidbody rb;
    float baseDrag = 0;
    float baseAngularDrag = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        baseDrag = rb.drag;
        baseAngularDrag = rb.angularDrag;
    }

    public void SlowDown()
    {
        DOTween.To(() => rb.angularDrag, (x) => rb.angularDrag = x, endValue, duration);
        DOTween.To(() => rb.drag, (x) => rb.drag = x, endValue, duration);
    }

    public void Release()
    {
        rb.angularDrag = baseAngularDrag;
        rb.drag = baseDrag;
    }
}
