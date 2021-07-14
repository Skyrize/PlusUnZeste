using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpacityController : MonoBehaviour
{
    [SerializeField] 
    Image[] images;

    private void Awake() {
        images = GetComponentsInChildren<Image>();
    }
    
    public void SetOpacity(float value)
    {
        foreach (Image image in images)
        {
            Color color = image.color;
            color.a = value;
            image.color = color;
        }
    }
}
