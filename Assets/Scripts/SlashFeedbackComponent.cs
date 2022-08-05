using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashFeedbackComponent : FeedbackComponent
{
    [SerializeField] override public FeedbackType feedbackType => FeedbackType.SLASH;
    [SerializeField] string m_audioFeedbackName = "";
    [SerializeField] AudioComponent m_audioComponent;
    [SerializeField] Vector3 m_particlePlayOffset;
    [SerializeField] GameObject m_particlePrefab;

    override public void PlayFeedback(GameObject _target)
    {
        GameObject particle = GameObject.Instantiate(m_particlePrefab, transform);
        particle.transform.position = _target.transform.position + m_particlePlayOffset;
        if (m_audioComponent) //TODO : remove when set
            m_audioComponent.Play(m_audioFeedbackName);
    }
}
