using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AbilityHandler : Global {

    /// <summary>
    /// in charge of the different abilities the player has
    /// </summary>

    //fields for general or multipule variable reference 
    private IPlayer player;
    private Heartbeat hb; //ref to the heartbeat component 

    [Header("Ability 0: Transfer")]
    [SerializeField] private GameObject abilityZeroPrefab; //prefab of the first bleed ability
    private AbilityTransfer powerZero; //ref to the abilityTransfer Compnent of the First ability
    private ParticleSystem part; //particle system attached to object
    protected ParticleSystem.Particle[] particles; //array of particles being controlled 

    [Header("Ability 1: Sprinting")]
    [SerializeField] private GameObject abilityOnePrefab;
    private AbilityAbsorb powerOne;
    [SerializeField] private float sprintMoveSpeed; //movement speed while sprinting
    [SerializeField] private float sprintJumpSpeed; //jump speed while sprinting
    private float orignMoveSpeed; //origional player move speed
    private float originJumpSpeed; //origional player jump speed
    [SerializeField] private int increaseRate; //rate heartrate increasess by each second
    [SerializeField] private int heartRateRemoved; //heartrate removed when sprinting stops
    private float heartRateAdded = 0; //heartrate that was added while sprinting
    private bool sprinting = false; //true when the player is sprinting
    private const int minSprintBPM = 170; //minimum heartrate the player can use the sprint ability at
    private const int sprintTime = 5; //time spent sprinting / exhausted
   /// private float sprintTimer = 0; //timer used to keep track of sprinting

    //idea for exhaust: have a sprint timer that increases while sprinting and then decreases while exhasted

    private static bool[] unlockedAbilities; //array for which abilities have been unlocked

    public AbilityTransfer PowerZero { get { return powerZero; } }

    // Use this for initialization
    void Awake () {
        //find player refs
        player = Player.GetComponent<IPlayer>();
        hb = player.Power.Heartbeat;

        //find power zero refs 
        powerZero = Instantiate(abilityZeroPrefab).GetComponent<AbilityTransfer>();
        powerZero.gameObject.SetActive(false);
        part = GetComponent<ParticleSystem>();
        //part.Stop();
        particles = new ParticleSystem.Particle[1]; //array of length one for checks if part has ANY active particles

        //get the normal speeds as the origional values
        orignMoveSpeed = player.MoveSpeed;
        originJumpSpeed = player.JumpSpeed;

        powerOne = Instantiate(abilityOnePrefab).GetComponent<AbilityAbsorb>();
        powerOne.gameObject.SetActive(false);

        //all abilities start out false
        if(unlockedAbilities == null)
        {
            unlockedAbilities = new bool[2]; //SET number of abilities here
            LockAll();
        }
        else
        {
            if (unlockedAbilities[0])
            {
                powerZero.gameObject.SetActive(true);
            }
            if (unlockedAbilities[1])
            {
                powerOne.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// set all the abilities to locked
    /// </summary>
    public void LockAll()
    {
        for (int i = 0; i < unlockedAbilities.Length; i++) { unlockedAbilities[i] = false; }
        Unlock(0); //unlock the ability transfer powers
        Unlock(1);
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
            switch (ability)
            {
                case 0:
                    powerZero.gameObject.SetActive(true);
                    break;
                case 1:
                    powerOne.gameObject.SetActive(true);
                    break;
            }
        }
    }

    void Update () {
        if (!paused)
        {
            if (unlockedAbilities[0])
            {
                if (hb.BPM > 1)
                {
                    powerZero.Useable = true;
                    //play the blled particle effect when mouse down or coming out of pause
                    if (Input.GetMouseButton(0) || (part.isPaused && part.GetParticles(particles) > 0))
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
            if (unlockedAbilities[1])
            {
                if (Input.GetMouseButton(1))
                {
                    sprinting = true;
                    player.Power.RestoreBPM(increaseRate * Time.deltaTime);
                    if(player.Power.Heartbeat.BPM < 200) { heartRateAdded += increaseRate * Time.deltaTime; }
                }
                //stop the sprinting
                else if (sprinting && Input.GetMouseButtonUp(1))
                {
                    //player.Power.RemoveBPM(heartRateAdded + heartRateRemoved);
                    sprinting = false;
                    heartRateAdded = 0;
                }
                player.MoveSpeed = sprinting ? sprintMoveSpeed : orignMoveSpeed;
                player.JumpSpeed = sprinting ? sprintJumpSpeed : originJumpSpeed;
                player.Animator.speed = sprinting ? sprintMoveSpeed / orignMoveSpeed : 1; //set animation speed to match 

                return;

                //BELOW: Trigger sprinting when BPM is above a value ---------------------------------------
                /*
                //start sprinting when BPM is above certian range and not already sprinting
                if (hb.BPM > minSprintBPM && !sprinting && sprintTimer == 0)
                {
                    sprinting = true;
                }
                if (sprinting)
                {
                    //increase timers and heartrate while sprinting
                    sprintTimer += Time.deltaTime;
                    player.Power.RestoreBPM(increaseRate * Time.deltaTime);
                    //start reset process after timer reaches limit
                    if (sprintTimer >= sprintTime)
                    {
                        sprinting = false;
                        player.Power.RemoveBPM(heartRateRemoved);
                        sprintTimer = sprintTime;
                    }
                }
                else
                {
                    //decrease the timer back to zero (cooldown)
                    sprintTimer = Mathf.Clamp(sprintTimer - Time.deltaTime, 0, sprintTime);
                }
                //set the move speeds to match 
                player.MoveSpeed = sprinting ? sprintMoveSpeed : orignMoveSpeed;
                player.JumpSpeed = sprinting ? sprintJumpSpeed : originJumpSpeed;
                player.Animator.speed = sprinting ? sprintMoveSpeed / orignMoveSpeed : 1; //set animation speed to match 
                */
                //-------------------------------------------------------------------------------------------
            }
        }
        //game is paused
        else
        {
            //pause the particle effect and prevent new particles from being made 
            if (unlockedAbilities[0])
            {
                part.Pause();
                powerZero.Useable = false;
            }  
        } 
    }
}
