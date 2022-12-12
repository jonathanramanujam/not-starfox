using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRange : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float enabledRange;
    private ParticleSystem[] particles;
    private bool isPlaying = false;
    private float distanceToPlayer;
    public float multiplier = 1;

    private void Start() {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = transform.position.magnitude - player.transform.position.magnitude;

        if (distanceToPlayer <= enabledRange)
        {
            if (isPlaying) { return; }

            PlayParticles();
            isPlaying = true;
        }
        else
        {
            if (!isPlaying) { return; }

            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }
            isPlaying = false;
        }
    }

    private void PlayParticles()
    {
        foreach (ParticleSystem particle in particles)
            {
				ParticleSystem.MainModule mainModule = particle.main;
				mainModule.startSizeMultiplier *= multiplier;
                mainModule.startSpeedMultiplier *= multiplier;
                mainModule.startLifetimeMultiplier *= Mathf.Lerp(multiplier, 1, 0.5f);
                particle.Clear();
                particle.Play();
            }
    }
}
