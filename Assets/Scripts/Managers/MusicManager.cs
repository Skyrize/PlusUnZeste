using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager = UnityEngine.SceneManagement.SceneManager
;

[RequireComponent(typeof(AudioManager))]
public class MusicManager : MonoBehaviour
{
    private AudioManager audioManager = null;

    public void PlayMenuMusic()
    {

    }

    private void Awake() {
        audioManager = GetComponent<AudioManager>();
    }

    private float timer = 0;
    private int musicIndex = 0;

    private void Update() {
        if (timer <= 0) {
            audioManager.Play(musicIndex);
            timer = audioManager.GetClipLenght();
            musicIndex++;
        if (musicIndex == 4)
            musicIndex = 0;

        } else {
            timer -= Time.deltaTime;
        }
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     Debug.Log("A retirer");
    //     if (Manager.GetActiveScene().name == "MainMenu") {
    //         PlayMenuMusic();
    //     } else {
    //         PlayGameMusic();
    //     } 
    // }

}
