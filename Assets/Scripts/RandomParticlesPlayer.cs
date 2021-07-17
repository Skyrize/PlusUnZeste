using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomParticlesPlayer : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] particles;
    [SerializeField]
    float waitTimer = 1f;
    [SerializeField]
    bool canMultiplay = false;

    [SerializeField] bool isPlaying = false;

    private void Awake() {
        particles = transform.GetComponentsInDirectChildren<ParticleSystem>();
        timer = new WaitForSeconds(waitTimer);
    }
    WaitForSeconds timer;

    IEnumerator Refresh()
    {
        yield return timer;
        isPlaying = false;
    }

    public void PlayRandom()
    {
        if (!canMultiplay && isPlaying)
            return;
        if (particles.Length == 0) {
            Debug.LogError($"Missing Particle systems as child of RandomParticlesPlayer {transform.name}");
        }
        var ps = particles[Random.Range(0, particles.Length)];
        ps.Play();
        if (!canMultiplay) {
            isPlaying = true;
            StartCoroutine(Refresh());
        }
        
    }

    public void PlayRandomAt(Vector3 position)
    {
        if (!canMultiplay && isPlaying)
            return;
        if (particles.Length == 0) {
            Debug.LogError($"Missing Particle systems as child of RandomParticlesPlayer {transform.name}");
        }
        var ps = particles[Random.Range(0, particles.Length)];
        ps.transform.position = position;
        ps.Play();
        if (!canMultiplay) {
            isPlaying = true;
            StartCoroutine(Refresh());
        }
        
    }

    public void PlayRandomAt(Transform target)
    {
        PlayRandomAt(target.position);
    }
}
