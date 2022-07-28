using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamageComponent))]
public class DamageFeedbackMediator : MonoBehaviour
{
    [SerializeField] FeedbackType m_feedbackType;

    private void Awake()
    {
        GetComponent<DamageComponent>().onDamage.AddListener(PlayFeedback);
    }
    
    void PlayFeedback(GameObject _target, float _damageAmount)
    {
        FeedbackManager.Instance.PlayFeedbackOfType(m_feedbackType, _target);
    }
}
