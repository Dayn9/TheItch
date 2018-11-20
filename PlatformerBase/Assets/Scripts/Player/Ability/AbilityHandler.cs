using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AbilityHandler : Global {

    /// <summary>
    /// in charge of the different abilities the player has
    /// </summary>

    [SerializeField] private GameObject abilityOnePrefab; //prefab of the first bleed ability
    private ParticleSystem part; //particle system attached to object
    protected ParticleSystem.Particle[] particles; //array of particles being controlled 

    private AbilityTransfer powerOne; //ref to the abilityTransfer Compnent of the First ability

    private Heartbeat hb; //ref to the heartbeat component 

    public AbilityTransfer PowerOne { get { return powerOne; } }
    //private GameObject ring;

    // Use this for initialization
    void Awake () {
        powerOne = Instantiate(abilityOnePrefab).GetComponent<AbilityTransfer>();
        part = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[1]; //array of length one for checks if part has ANY active particles

        hb = Player.GetComponent<IPlayer>().Power.Heartbeat;
    }

    // Update is called once per frame
    void Update () {

        if (!paused)
        {
            if(hb.BPM > 1)
            {
                powerOne.Useable = true;
                //play the blled particle effect when mouse down or coming out of pause
                if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(0) || (part.isPaused && part.GetParticles(particles) > 0))
                {
                    part.Play();
                    powerOne.AudioPlayer.PlaySound("ContinueSparkle");
                }
                else
                {
                    part.Stop();
                }
            }
            else
            {
                part.Stop();
                powerOne.Useable = false;
            }
        }
        else
        {
            part.Pause();
            powerOne.Useable = false;
        } 
    }
}
