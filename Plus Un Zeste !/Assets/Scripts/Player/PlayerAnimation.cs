using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class ScaleAnimation
{
    private Sequence animation = null;
    [SerializeField] private string _name = "default name";
    [SerializeField] private float duration = 1f;
    [SerializeField] private Vector3 scale = new Vector3(2f, 2f, 2f);
    [SerializeField] private Ease ease = Ease.OutQuad;

    public string name {
        get {
            return _name;
        }
    }

    public void Initialize(Transform target)
    {
        animation = DOTween.Sequence();
        animation.Append(target.DOScale(scale, duration / 2));
        animation.Append(target.DOScale(new Vector3(2f, 2f, 2f), duration / 2));
        animation.SetAutoKill(false);
        animation.Pause();
        
    }

    public void Play()
    {
        Debug.Log("playing anim : " + name);
        animation.Restart();
    }
}

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private ScaleAnimation[] animations = null;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ScaleAnimation animation in animations)
        {
            animation.Initialize(transform);
        }
    }

    public void Play(int animationIndex)
    {
        animations[animationIndex].Play();
    }

    public void Play(string animationName)
    {
        foreach (ScaleAnimation anim in animations)
        {
            if (anim.name == animationName) {
                anim.Play();
                return;
            }
        }
        Debug.LogWarning("Cant find animation named '" + animationName + "' !");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
