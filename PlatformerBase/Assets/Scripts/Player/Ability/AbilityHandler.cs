using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AbilityHandler : Global {

    /// <summary>
    /// in charge of the different abilities the player has
    /// </summary>

    [SerializeField] private GameObject abilityZeroPrefab; //prefab of the first bleed ability
    private ParticleSystem part; //particle system attached to object
    protected ParticleSystem.Particle[] particles; //array of particles being controlled 

    private AbilityTransfer powerZero; //ref to the abilityTransfer Compnent of the First ability

    private Heartbeat hb; //ref to the heartbeat component 

    public AbilityTransfer PowerZero { get { return powerZero; } }

    private static bool[] unlockedAbilities; //array for which abilities have been unlocked

    // Use this for initialization
    void Awake () {
        powerZero = Instantiate(abilityZeroPrefab).GetComponent<AbilityTransfer>();
        powerZero.gameObject.SetActive(false);
        part = GetComponent<ParticleSystem>();
        //part.Stop();
        particles = new ParticleSystem.Particle[1]; //array of length one for checks if part has ANY active particles

        hb = Player.GetComponent<IPlayer>().Power.Heartbeat;

        //all abilities start out false
        if(unlockedAbilities == null)
        {
            unlockedAbilities = new bool[1]; //SET number of abilities here
            LockAll();
        }
    }

    /// <summary>
    /// set all the abilities to locked
    /// </summary>
    public void LockAll()
    {
        for (int i = 0; i < unlockedAbilities.Length; i++) { unlockedAbilities[i] = false; }
    }


    /// <summary>
    /// used to unlock a specific ability the player can posess
    /// </summary>
    /// <param name="ability"></param>
    public void Unlock(int ability)
    {
        if(ability >= 0 && ability < unlockedAbilities.Length)
        {
            unlockedAbilities[ability] = true;
            if (unlockedAbilities[0])
            {
                powerZero.gameObject.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (!paused)
        {
            if (unlockedAbilities[0])
            {
                if (hb.BPM > 1)
                {
                    powerZero.Useable = true;
                    //play the blled particle effect when mouse down or coming out of pause
                    if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(0) || (part.isPaused && part.GetParticles(particles) > 0))
                    {
                        part.Play();
                        powerZero.AudioPlayer.PlaySound("ContinueSparkle");
                    }
                    else
                    {
                        part.Stop();
                    }
                }
                else
                {
                    part.Stop();
                    powerZero.Useable = false;
                }
            }
        }
        else
        {
            if (unlockedAbilities[0])
            {
                part.Pause();
                powerZero.Useable = false;
            }  
        } 
    }
}
