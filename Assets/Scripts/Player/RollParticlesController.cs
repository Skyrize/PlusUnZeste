using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RollParticlesController : MonoBehaviour
{
    [Header("Settings")]
    private float threshold = 0.3f;
    private float speed = 2f;
    [Header("References")]
    [SerializeField] private Rigidbody rb = null;
    private ParticleSystem particleEmitter = null;

    private void Awake() {
        particleEmitter = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var main = particleEmitter.main;
        Vector3 velocity = rb.GetComponent<Rigidbody>().velocity;

        if (velocity.sqrMagnitude <= threshold) {
            if (particleEmitter.isPlaying)
                particleEmitter.Stop();
        } else {
            if (!particleEmitter.isPlaying)
                particleEmitter.Play();
            transform.rotation = Quaternion.LookRotation(-velocity);
            main.startSpeed = velocity.magnitude * speed;
        }
    }
}
