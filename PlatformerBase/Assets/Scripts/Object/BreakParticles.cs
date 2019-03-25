using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BreakParticles : MonoBehaviour
{
    private ParticleSystem part;
    private ParticleSystem.ShapeModule partShape;

    private ParticleSystem.Particle[] particles;

    void Awake()
    {
        part = GetComponent<ParticleSystem>();
        partShape = part.shape;

        particles = new ParticleSystem.Particle[16];
    }

    public void BreakAt(Vector3Int tilePosition)
    {
        partShape.position = tilePosition + (Vector3)Vector2.one / 2;
        part.Emit(16);


        Vector2 pos = Vector2.zero;

        part.GetParticles(particles, 16, part.particleCount - 16);

        for(int i = 0; i < particles.Length; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            pos.x = 0.125f + ((i % 4) / 4.0f);
            pos.y = 0.125f + (Mathf.Floor(i / 4)) / 4;

            particle.position = tilePosition + (Vector3)pos;

            particles[i] = particle;
        }

        part.SetParticles(particles, 16, part.particleCount - 16);
    }


}
