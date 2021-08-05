using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float magnitude = 0.06f;
    [SerializeField] private float ratioWeight = .5f;

    private bool done = true;
    
    public void Shake(float ratio)
    {
        if (done)
            StartCoroutine(_Shake(ratio));
    }

    private IEnumerator _Shake(float ratio)
    {
        Vector3 originalPos = transform.localPosition;
        float realRatio = ratio * ratioWeight;
        float elapsed = 0f;

        done = false;
        if (realRatio <= 0) {
            realRatio = 1;
            Debug.LogError($"realRatio <= 0 ? Weird .. {ratio} x {ratioWeight}");
        }
        while (elapsed < duration * realRatio) {
            if (Time.deltaTime == 0) {
                yield return null;
                continue;
            }
            float x = Random.Range(-1, 1) * magnitude * realRatio;
            float y = Random.Range(-1, 1) * magnitude * realRatio;

            transform.localPosition = new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        done = true;
        transform.localPosition = originalPos;
    }

}