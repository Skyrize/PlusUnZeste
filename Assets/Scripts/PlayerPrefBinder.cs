using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class FloatEvent : UnityEvent<float>
{
}

// public class FloatPrefBinder {
//     public string name = "default";
//     public FloatEvent onLoad = new FloatEvent();

//     public void Save()
// }

public class PlayerPrefBinder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string prefName = "";
    [Header("Events")]
    [SerializeField] private FloatEvent onFloatEvent = new FloatEvent();

    // Start is called before the first frame update
    void Awake()
    {
        onFloatEvent.Invoke(PlayerPrefs.GetFloat(prefName, 0.5f));
    }

    public void Save(float newValue)
    {
        PlayerPrefs.SetFloat(prefName, newValue);
    }
}
