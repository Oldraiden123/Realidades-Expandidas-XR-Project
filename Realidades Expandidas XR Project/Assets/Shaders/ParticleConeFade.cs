using UnityEngine;
using System.Collections.Generic;

public class ParticleConeFadeOnTrigger : MonoBehaviour
{
    public float insideAlpha = 0.2f;   // Opacity inside cone
    public float outsideAlpha = 1f;    // Opacity outside cone

    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        int count = ps.particleCount;

        if (particles == null || particles.Length < count)
            particles = new ParticleSystem.Particle[count];

        ps.GetParticles(particles);

        // Get particles inside trigger
        List<ParticleSystem.Particle> insideParticles = new List<ParticleSystem.Particle>();
        ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, insideParticles);

        foreach (var particle in insideParticles)
        {
            int index = System.Array.IndexOf(particles, particle);
            if (index >= 0)
            {
                Color c = particles[index].startColor;
                c.a = insideAlpha;
                particles[index].startColor = c;
            }
        }

        // Get particles outside trigger
        List<ParticleSystem.Particle> outsideParticles = new List<ParticleSystem.Particle>();
        ps.GetTriggerParticles(ParticleSystemTriggerEventType.Outside, outsideParticles);

        foreach (var particle in outsideParticles)
        {
            int index = System.Array.IndexOf(particles, particle);
            if (index >= 0)
            {
                Color c = particles[index].startColor;
                c.a = outsideAlpha;
                particles[index].startColor = c;
            }
        }

        // Apply changes
        ps.SetParticles(particles, count);
    }
}
