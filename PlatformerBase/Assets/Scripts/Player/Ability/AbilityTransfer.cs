using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AbilityTransfer : Global {

    [SerializeField] private float particleSpeed; 
    [SerializeField] private float heartrateUsed;

    private CircleCollider2D coll; //ref to this objects collider
    //private Animator anim; //ref to this objects animator
    //private SpriteRenderer rend; //ref to this objects sprite renderer

    private ParticleSystem part; //ref to this objects particle system
    private ParticleSystemRenderer partRend;

    private SpriteRenderer playerRend; //ref to players sprite renderer

    private ParticleSystem.Particle[] particles; //array of particles being controlled 
    private int sentParticles;
    private bool sending; //true when particles arde being sent to a location
    private Vector2 target;

    private HeartbeatPower hbPower; //red to the hearybeat power script

    // Use this for initialization
    void Awake () {
        coll = GetComponent<CircleCollider2D>();
        //anim = GetComponent<Animator>();
        //rend = GetComponent<SpriteRenderer>();

        part = GetComponent<ParticleSystem>();
        partRend = GetComponent<ParticleSystemRenderer>();
        particles = new ParticleSystem.Particle[part.main.maxParticles];

        playerRend = Player.GetComponent<SpriteRenderer>();
        hbPower = Player.GetComponent<IPlayer>().Power;

        coll.enabled = false;
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
        part.Emit(minNum);
        sentParticles = minNum;

       
        /*
        //loop through all particles
        int numParticles = part.GetParticles(particles);

        sentParticles = numParticles;
        
        for(int i = 0; i < numParticles; i++)
        {
            ParticleSystem.Particle particle = particles[i]; //get the individual particle

            Vector2 moveVector = ((Vector3)target - particle.position);

            particle.remainingLifetime += Time.deltaTime; //keep particle alive
            particle.velocity =  moveVector * Time.deltaTime * particleSpeed; //move the particle
            particles[i] = particle; //set the particle's data back into particles array
        }
        */
        sending = true;
        //part.SetParticles(particles, numParticles); //apply changes to particle system
    }
	
	// Update is called once per frame
	void Update () {
        if (!paused)
        {
            if (part.isPaused) { part.Play(); }
            if (sending)
            {
                //loop through all particles
                int numParticles = part.GetParticles(particles);
                for (int i = 0; i < numParticles; i++)
                {
                    ParticleSystem.Particle particle = particles[i];

                    particle.remainingLifetime += Time.deltaTime;
                    particle.velocity += ((((Vector3)target - particle.position).normalized * particleSpeed) - particle.velocity) * Time.deltaTime;

                    if (Vector2.Distance(particle.position, target) < 0.5f)
                    {
                        particle.remainingLifetime = 0;
                        particle.velocity = Vector3.zero;
                    }
                    particles[i] = particle; //set the particle's data back into particles array
                }
                
                if(sentParticles <= 0 || numParticles <= 0)
                {
                    sending = false;
                }
                part.SetParticles(particles, numParticles); //apply changes to particle system
            }

            if (Input.GetKeyUp(KeyCode.X) || Input.GetMouseButtonUp(0))
            {
                //try and turn set regular color when key released
                hbPower.SetDamageColor(false);
            }
            if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(0))
            {
                //anim.SetBool("Grow", true);
                //anim.SetBool("Shrink", false);
                coll.enabled = true;
                part.Play();

                //snap to players position
                transform.position = Player.transform.position;

                //set render properties to be right behind player
                //rend.sortingLayerID = playerRend.sortingLayerID;
                //rend.sortingOrder = playerRend.sortingOrder - 2;
                partRend.sortingLayerID = playerRend.sortingLayerID;
                partRend.sortingOrder = playerRend.sortingOrder -1;

                //set 
                hbPower.RemoveBPM(heartrateUsed * Time.deltaTime);
                hbPower.SetDamageColor(true);
            }
            else
            { 
                //anim.SetBool("Grow", false);
                //anim.SetBool("Shrink", true);
                coll.enabled = false;
                part.Stop();
            }
        }
        else
        {
            part.Pause();
        }
	}
}
