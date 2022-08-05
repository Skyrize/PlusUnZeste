using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnFeedbackComponent : FeedbackComponent
{
    [SerializeField] override public FeedbackType feedbackType => FeedbackType.BURN;
    [SerializeField] string m_audioFeedbackName = "";
    [SerializeField] AudioComponent m_audioComponent;
    [SerializeField] Vector3 m_particlePlayOffset;
    [SerializeField] RandomParticlesPlayer m_randomParticlesPlayer;

    override public void PlayFeedback(GameObject _target)
    {
        if (m_randomParticlesPlayer) //TODO : remove when set
            m_randomParticlesPlayer.PlayRandomAt(_target.transform.position + m_particlePlayOffset);
        if (m_audioComponent) //TODO : remove when set
            m_audioComponent.Play(m_audioFeedbackName);
    }
}
