using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WinUI : MonoBehaviour
{
    public float duration = 1.5f;
    public Ease easing;
    public void Win()
    {
        GetComponent<Image>().DOFade(1, duration).SetEase(easing);
    }
}
