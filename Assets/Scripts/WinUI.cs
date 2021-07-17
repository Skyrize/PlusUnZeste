using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class WinUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float duration = 1.5f;
    [SerializeField] Ease easing;
    [SerializeField] float loseDuration = 1.5f;
    [SerializeField] Ease loseEasing;
    public UnityEvent onRespawnHalf = new UnityEvent();
    public UnityEvent onLoseHalf = new UnityEvent();
    Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }
    public void Win()
    {
        var tween = image.DOFade(1, duration).SetEase(easing);
    }

    public void Respawn()
    {
        Sequence sequence = DOTween.Sequence();
        var tween = image.DOFade(1, loseDuration / 2f).SetEase(loseEasing);
        tween.onComplete += () => onRespawnHalf.Invoke();
        sequence.Append(tween);
        sequence.Append(image.DOFade(0, loseDuration / 2f).SetEase(loseEasing));
        sequence.Play();
    }

    public void Lose()
    {
        Sequence sequence = DOTween.Sequence();
        var tween = image.DOFade(1, loseDuration / 2f).SetEase(loseEasing);
        tween.onComplete += () => onLoseHalf.Invoke();
        sequence.Append(tween);
        sequence.Append(image.DOFade(0, loseDuration / 2f).SetEase(loseEasing));
        sequence.Play();
        // .DOFade(1, loseDuration / 2f).SetEase(loseEasing);
    }
}
