using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticle : Global {

    [SerializeField] private float particleSpeed;
    private const int particleMultiplier = 3;

    /// <summary>
    /// methods and properties for controlling the movement of blood particles
    /// </summary>
    /// 
    protected ParticleSystem part; //ref to this objects particle system
    protected ParticleSystemRenderer partRend;
    protected ParticleSystem.Particle[] particles; //array of particles being controlled 

    protected int sentParticles;
    protected bool sending; //true when particles arde being sent to a location

    protected Vector2 target;
    protected Transform targetTrans;

    protected HeartbeatPower hbPower; //ref to the hearybeat power script

    private bool useable = true; //true when object can use particle effect (better name needed)
    public bool Useable { set { useable = value; } }

    private const float overshoot = 0.75f;
    private const float slowRadius = 6;

    protected AudioPlayer audioPlayer; //references gotten in inheriting classes

    protected virtual void Awake()
    {
        part = GetComponent<ParticleSystem>();
        partRend = GetComponent<ParticleSystemRenderer>();
        particles = new ParticleSystem.Particle[part.main.maxParticles];

        hbPower = Player.GetComponent<IPlayer>().Power;

        part.Stop();
    }

    
    public void SendParticlesTo(Vector2 target, int minNum)
    {
        if (useable)
        {
            this.target = target;
            //emit additional particles 
            part.Emit(minNum * particleMultiplier);
            sentParticles = minNum * particleMultiplier;

            //start sending p[articles to point
            sending = true;

            audioPlayer.PlaySound("bloodSparkle");
        }
    }

    /// <summary>
    /// Send Particles to a specified world positions
    /// </summary>
    /// <param name="targets">target positions</param>
    /// <param name="minNum">minimum number of particles</param>
    public void SendParticlesTo(Transform target, int minNum)
    {
        if (useable)
        {
            //part.Stop(); //stop all the current particles 
            targetTrans = target;

            //emit additional particles 
            part.Emit(minNum * particleMultiplier);
            sentParticles += minNum * particleMultiplier;

            //start sending p[articles to point
            sending = true;

            audioPlayer.PlaySound("bloodSparkle");
        }
    }

    protected void MoveParticles()
    {
        if (!paused)
        {
            if (sending)
            {
                if (part.isPaused) { part.Play();  }

                //loop through all particles
                int numParticles = part.GetParticles(particles);
                target = targetTrans.position;
                for (int i = 0; i < numParticles; i++)
                {
                    ParticleSystem.Particle particle = particles[i];
                    //particle.remainingLifetime += Time.deltaTime; //keep particle alive
                    Vector3 moveVector = ((Vector3)target - particle.position);
                    moveVector += moveVector.normalized * overshoot;

                    particle.velocity += ((moveVector.normalized * particleSpeed) - particle.velocity) * Time.deltaTime;
                    particle.velocity *= Mathf.Clamp((moveVector.magnitude + slowRadius) / 10 , slowRadius * 0.1f , 1);

                    if (moveVector.magnitude - overshoot < 1f)
                    {
                        particle.remainingLifetime = 0;
                        particle.velocity = Vector3.zero;
                        sentParticles -= 1;
                    }
                    particles[i] = particle; //set the particle's data back into particles array
                                             //Debug.Log("Seth is dim");
                }

                if (sentParticles <= 0 || numParticles <= 0)
                {
                    sentParticles = 0;
                    sending = false;
                    part.Stop();
                }
                part.SetParticles(particles, numParticles); //apply changes to particle system
            }
        }
        else
        {
            part.Pause();
        }
    }
}
