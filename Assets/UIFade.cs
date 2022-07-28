using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class UIFade : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool m_fadeIn;
    [SerializeField] float m_fadeInDuration = 1.5f;
    [SerializeField] Ease m_fadeInEasing;
    [SerializeField] bool m_fadeOut;
    [SerializeField] float m_fadeOutDuration = 1.5f;
    [SerializeField] Ease m_fadeOutEasing;
    [HideInInspector] public UnityEvent onFadeInStart = new UnityEvent();
    [HideInInspector] public UnityEvent onFadeInEnd = new UnityEvent();
    [HideInInspector] public UnityEvent onFadeOutStart = new UnityEvent();
    [HideInInspector] public UnityEvent onFadeOutEnd = new UnityEvent();
    
    Image m_image;

    private void Awake() {
        m_image = GetComponent<Image>();
    }
    public void Play()
    {
        Sequence sequence = DOTween.Sequence();
        if (m_fadeIn)
        {
            var fadeInTween = m_image.DOFade(1, m_fadeInDuration).SetEase(m_fadeInEasing);
            fadeInTween.onPlay += onFadeInStart.Invoke;
            fadeInTween.onComplete += OnFadeInComplete;
            sequence.Append(fadeInTween);
        }
        if (m_fadeOut)
        {
            var fadeOutTween = m_image.DOFade(0, m_fadeOutDuration).SetEase(m_fadeOutEasing);
            fadeOutTween.onPlay += onFadeOutStart.Invoke;
            fadeOutTween.onComplete += onFadeOutEnd.Invoke;
            sequence.Append(fadeOutTween);
        }
        sequence.Play();
    }

    void OnFadeInComplete()
    {
        onFadeInEnd.Invoke();
    }
}
