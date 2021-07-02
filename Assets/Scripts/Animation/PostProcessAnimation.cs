using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using System;

[System.Serializable]
public struct FloatAnimation
{
    public string name;
    public float targetValue;
    public float animationDuration;
    
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }
    
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    
    public static bool operator ==(FloatAnimation f1, FloatAnimation f2) { return f1.name == f2.name && f1.targetValue == f2.targetValue && f1.animationDuration == f2.animationDuration; }
    public static bool operator !=(FloatAnimation f1, FloatAnimation f2) { return !(f1==f2); }
}

// [RequireComponent(typeof(Volume))]
public class PostProcessAnimation : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField]
    private FloatAnimation[] floatAnimations = null;
    // private Volume volume;

    private void Awake() {
        // volume = GetComponent<Volume>();
    }
    private IEnumerator AnimateExposure(float startingValue, float targetValue, float animationDuration)
    {
        // RenderSettings.skybox
        // volume.GetVolumeComponent<DepthOfField>().
        // volume.GetVolumeComponent<Exposure>().Override()
        Debug.Log("Start animate");
        float elapsedTime = 0.0f;
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(Mathf.Lerp(startingValue, targetValue, Mathf.Clamp01(elapsedTime / animationDuration)));
            

            RenderSettings.skybox.SetFloat("_Exposure", Mathf.Lerp(startingValue, targetValue, Mathf.Clamp01(elapsedTime / animationDuration)));
            yield return new WaitForEndOfFrame();
        }
    }

    public void AnimateExposure(string floatAnimationName)
    {
        FloatAnimation animation = Array.Find(floatAnimations, (anim) => anim.name == floatAnimationName);

        if (animation == null) {
            Debug.LogException(new System.Exception("Can't find animation named " + floatAnimationName));
            return;
        }
        StartCoroutine(AnimateExposure(RenderSettings.skybox.GetFloat("_Exposure"), animation.targetValue, animation.animationDuration));
    }
}
