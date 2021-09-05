using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    public void Load()
    {
        onFloatEvent.Invoke(PlayerPrefs.GetFloat(prefName, 0.5f));
    }

    // Start is called before the first frame update
    void Awake()
    {
        var slider = GetComponent<Slider>();

        slider.onValueChanged.AddListener(this.Save);
        var mixer = FindObjectOfType<MixerManager>();

        if (transform.name.Contains("Master"))
            slider.onValueChanged.AddListener(mixer.SetMasterVolume);
        if (transform.name.Contains("Music"))
            slider.onValueChanged.AddListener(mixer.SetMusicVolume);
        if (transform.name.Contains("Effects"))
            slider.onValueChanged.AddListener(mixer.SetEffectsVolume);

        Load();
    }

    public void Save(float newValue)
    {
        PlayerPrefs.SetFloat(prefName, newValue);
    }
}
