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
    Tween m_dragTweenAnim;
    Tween m_angularDragTweenAnim;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        baseDrag = rb.drag;
        baseAngularDrag = rb.angularDrag;
    }

    public void SlowDown(float value)
    {
        m_angularDragTweenAnim?.Kill();
        m_angularDragTweenAnim = DOTween.To(() => baseAngularDrag, (x) => rb.angularDrag = x, value, duration);
        m_dragTweenAnim?.Kill();
        m_dragTweenAnim = DOTween.To(() => baseDrag, (x) => rb.drag = x, value, duration);
    }

    public void Release()
    {
        m_angularDragTweenAnim?.Kill();
        m_dragTweenAnim?.Kill();
        rb.angularDrag = baseAngularDrag;
        rb.drag = baseDrag;
    }
}
