using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Image))]
public class UIFillBar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Gradient gradient;
    [SerializeField] private float decreaseSpeed = 0.3f;
    [SerializeField] private Ease decreaseEase = Ease.OutQuint;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.color = gradient.Evaluate(1);
    }

    public void SetFill(float _amount)
    {
        image.color = gradient.Evaluate(_amount);
        image.DOFillAmount(_amount, decreaseSpeed).SetEase(decreaseEase);
    }

}