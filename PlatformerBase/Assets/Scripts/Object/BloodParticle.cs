using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : Global {

    [SerializeField] private float particleSpeed;
    private const int particleMultiplier = 3;

    /// <summary>
    /// methods and properties for controlling the movement of blood particles
    /// </summary>
    protected ParticleSystem part; //ref to this objects particle system
    protected ParticleSystemRenderer partRend;
    protected ParticleSystem.Particle[] particles; //array of particles being controlled 

    protected int sentParticles;
    protected bool sending; //true when particles arde being sent to a location
    protected Vector2 target;

    protected HeartbeatPower hbPower; //ref to the hearybeat power script

    protected virtual void Awake()
    {
        part = GetComponent<ParticleSystem>();
        partRend = GetComponent<ParticleSystemRenderer>();
        particles = new ParticleSystem.Particle[part.main.maxParticles];

        hbPower = Player.GetComponent<IPlayer>().Power;

        part.Stop();
    }

    /// <summary>
    /// Send Particles to a specified world positions
    /// </summary>
    /// <param name="targets">target positions</param>
    /// <param name="minNum">minimum number of particles</param>
    public void SendParticlesTo(Vector2 target, int minNum)
    {
        this.target = target;
        //emit additional particles 
        part.Emit(minNum * particleMultiplier);
        sentParticles = minNum * particleMultiplier;

        //start sending p[articles to point
        sending = true;
        
    }

    protected void MoveParticles()
    {
        //if (part.isPaused) { part.Play(); }
        if (sending)
        {
            //loop through all particles
            int numParticles = part.GetParticles(particles);
            for (int i = 0; i < sentParticles; i++)
            {
                ParticleSystem.Particle particle = particles[i];

                particle.remainingLifetime += Time.deltaTime;
                particle.velocity += ((((Vector3)target - particle.position).normalized * particleSpeed) - particle.velocity) * Time.deltaTime;

                if (Vector2.Distance(particle.position, target) < 0.5f)
                {
                    particle.remainingLifetime = 0;
                    particle.velocity = Vector3.zero;
                    sentParticles -= 1;
                }
                particles[i] = particle; //set the particle's data back into particles array
            }

            if (sentParticles <= 0 || numParticles <= 0)
            {
                sending = false;
            }
            part.SetParticles(particles, numParticles); //apply changes to particle system
        }
    }
}
