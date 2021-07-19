using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class OpacityController : MonoBehaviour
{
    CanvasGroup canvasGroup;
    float fadeDuration = 1;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void SetOpacity(float value)
    {
        canvasGroup.alpha = value;
    }

    public void AnimateOpacity(float target)
    {
        canvasGroup.DOFade(target, fadeDuration);
    }
    public void AnimateOpacity(float target, float duration)
    {
        canvasGroup.DOFade(target, duration);
    }
}
