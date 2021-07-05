using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioManager))]
public class MusicManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fadeOutDuration = 0.8f;
    [SerializeField] private float fadeInDuration = 0.4f;
    [SerializeField] private string levelMusic = "GameMusic1";
    private string current = "";
    private AudioManager audioManager = null;
    private AudioSource audioSource = null;

    public void PlayInstant(string name) {
        if (current == name)
            return;
        audioSource.Stop();
        current = name;
        audioManager.Play(name);
    }

    IEnumerator _PlaySmooth(string name)
    {
        float volume = audioSource.volume;
        audioSource.DOFade(0, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);
        audioSource.Stop();
        audioManager.Play(name);
        audioSource.DOFade(volume, fadeInDuration);
    }

    public void PlaySmooth(string name) {
        if (current == name)
            return;
        current = name;
        StartCoroutine(_PlaySmooth(name));
    }

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        audioSource = GetComponent<AudioSource>();
        audioManager.Play(levelMusic);
    }

}
