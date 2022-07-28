using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FeedbackType
{
    BURN,
    SLASH,
    BOING
}

public abstract class FeedbackComponent : MonoBehaviour
{
    abstract public void PlayFeedback(GameObject _target);
    abstract public FeedbackType feedbackType {
        get;
    }
}

public class FeedbackManager : MonoBehaviour
{

    private static FeedbackManager instance = null;
    public static FeedbackManager Instance => instance;

    private Dictionary<FeedbackType, List<FeedbackComponent>> m_feedbacks = new Dictionary<FeedbackType, List<FeedbackComponent>>();
    
    private void Awake() {
        if (instance)
        {
            throw new UnityException("There is already an Instance of FeedbackManager");
        }
        instance = this;

        FeedbackComponent[] feedbacks = GetComponentsInChildren<FeedbackComponent>(true);
        foreach (FeedbackComponent feedback in feedbacks)
        {
            if (!m_feedbacks.ContainsKey(feedback.feedbackType))
            {
                m_feedbacks.Add(feedback.feedbackType, new List<FeedbackComponent>());
            }
            m_feedbacks[feedback.feedbackType].Add(feedback);
        }
    }

    public void PlayFeedbackOfType(FeedbackType _feedbackType, GameObject _target)
    {
        foreach (FeedbackComponent feedback in m_feedbacks[_feedbackType])
        {
            feedback.PlayFeedback(_target);
        }
    }
}
