using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(Light))]
public class LightAnimation : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField]
    private FloatAnimation[] floatAnimations = null;
    private Light target;

    private void Awake() {
        target = GetComponent<Light>();
    }

    public void AnimateIntensity(string animationName)
    {
        FloatAnimation animation = Array.Find(floatAnimations, (anim) => anim.name == animationName);

        if (animation == null) {
            Debug.LogException(new System.Exception("Can't find animation named " + animationName));
            return;
        }
        AnimateIntensity(animation.targetValue, animation.animationDuration);
    }
    
    public void AnimateIntensity(float targetIntensity, float duration)
    {
        target.DOIntensity(targetIntensity, duration);
    }

}
