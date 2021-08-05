using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBinderCollider : MonoBehaviour
{
    public string defaultEventName = "";
    public void CallBinderOnTarget(Transform target, string eventName)
    {
        EventBinder binder = target.GetComponent<EventBinder>();

        if (binder) {
            binder.CallEvent(eventName);
        } else {
            Debug.LogError($"No EventBinder on {target.name}");
        }
    }

    public void CallBinderOnTarget(Transform target)
    {
        EventBinder binder = target.GetComponent<EventBinder>();

        if (binder) {
            binder.CallEvent(defaultEventName);
        } else {
            Debug.LogError($"No EventBinder on {target.name}");
        }
    }
}
