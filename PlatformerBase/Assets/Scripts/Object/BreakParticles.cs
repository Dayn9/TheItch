using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BreakParticles : MonoBehaviour
{
    /// <summary>
    /// Controls the broken particles
    /// </summary>

    private ParticleSystem part; //ref to the particle system component 

    private ParticleSystem.Particle[] particles; //array of particles to control

    void Awake()
    {
        part = GetComponent<ParticleSystem>();

        //create the particle array
        particles = new ParticleSystem.Particle[16];
    }

    /// <summary>
    /// Create the broken particles and place them
    /// </summary>
    /// <param name="tilePosition"></param>
    public void BreakAt(Vector3Int tilePosition)
    {
        // <<< 2x2 break

        //partShape.position = tilePosition + (Vector3)Vector2.one / 2;
        part.Emit(4);
        //get the last 4 particles emitted
        part.GetParticles(particles, 4, part.particleCount - 4);

        for (int i = 0; i < particles.Length; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            //set the new particle position
            particle.position = tilePosition +
                new Vector3(0.25f + ((i % 2) / 2.0f),
                            0.25f + (Mathf.Floor(i / 2)) / 2,
                            0);

            //replace the particle
            particles[i] = particle;
        }
        //set the particles back into the system
        part.SetParticles(particles, 4, part.particleCount - 4);
        



        /*
        <<< 4x4 break >>>

        //partShape.position = tilePosition + (Vector3)Vector2.one / 2;
        part.Emit(16);
        //get the last 16 particles emitted
        part.GetParticles(particles, 16, part.particleCount - 16);

        for(int i = 0; i < particles.Length; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            //set the new particle position
            particle.position = tilePosition + 
                new Vector3(0.125f + ((i % 4) / 4.0f), 
                            0.125f + (Mathf.Floor(i / 4)) / 4, 
                            0);

            //replace the particle
            particles[i] = particle;
        }
        //set the particles back into the system
        part.SetParticles(particles, 16, part.particleCount - 16);
        */
    }


}
