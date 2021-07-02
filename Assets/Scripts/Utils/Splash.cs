using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform audioSource = null;

    private AudioManager audioManager = null;

    private Transform swimmer = null;
    private float timer = 0;
    private float clipLenght = 0;

    private void Awake() {
        audioManager = audioSource.GetComponent<AudioManager>();
    }

    public void PlayDiveAt(Transform target)
    {
        audioSource.transform.position = target.position;
        audioManager.Play("Dive");
        if (target.tag == "Player") {
            clipLenght = audioManager.GetClipLenght("Swim");
            timer = clipLenght;
            swimmer = target;
        }
    }

    public void GetOut(Transform target)
    {
        if (target == swimmer) {

            swimmer = null;
        }
    }

    private void Update() {
        if (swimmer) {
            if (timer <= 0) {
                audioManager.Play("Swim");
                timer = clipLenght;
                audioSource.position = swimmer.position;
            } else {
                timer -= Time.deltaTime;
            }
        }
    }
}
