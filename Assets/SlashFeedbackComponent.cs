using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashFeedbackComponent : FeedbackComponent
{
    [SerializeField] override public FeedbackType feedbackType => FeedbackType.SLASH;
    [SerializeField] string m_audioFeedbackName = "";
    [SerializeField] AudioComponent m_audioComponent;
    [SerializeField] Vector3 m_particlePlayOffset;
    [SerializeField] ParticleSystem m_particleSystem;

    override public void PlayFeedback(GameObject _target)
    {
        transform.position = _target.transform.position;
        if (m_particleSystem) //TODO : remove when set
            m_particleSystem.Play();
        if (m_audioComponent) //TODO : remove when set
            m_audioComponent.Play(m_audioFeedbackName);
    }
}
