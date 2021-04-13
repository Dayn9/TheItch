using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AbilityHandler : MonoBehaviour {

    /// <summary>
    /// in charge of the different abilities the player has
    /// </summary>

    //fields for general or multipule variable reference 
    private Jump player;

    [Header("Ability 0: Transfer")]
    [SerializeField] private GameObject abilityZeroPrefab; //prefab of the first bleed ability
    private AbilityTransfer powerZero; //ref to the abilityTransfer Compnent of the First ability
    private ParticleSystem part; //particle system attached to object
    protected ParticleSystem.Particle[] particles; //array of particles being controlled 

    [Header("Ability 1: Absorb")]
    private ParticleSystem partAbsorb; //base absorb particle system
    [SerializeField] private GameObject abilityOnePrefab;
    private AbilityAbsorb powerOne;

    [Header("Ability 2: Sprinting")]
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
    // private float sprintTimer = 0; //timer used to keep track of sprinting
    [SerializeField] private float sprintHealRate;

    [Header("Ability 3: Swimming")]
    [SerializeField] private int rateOverDistance; //how many water particles to emit
    [SerializeField] [Range(0, 1)] private float moveWeight; //weight given to moveVeclocity
    private ParticleSystem partWater; //ref to the water particle system
    [SerializeField] private float waterHealRate;

    private bool inside = false;

    //idea for exhaust: have a sprint timer that increases while sprinting and then decreases while exhasted
    private static bool[] unlockedAbilities; //array for which abilities have been unlocked

    /// <summary>
    /// checks if an ability has been unlocked
    /// </summary>
    /// <param name="ability">ability number to check</param>
    /// <returns>true if ability is unlocked</returns>
    public static bool IsUnlocked(int ability)
    {
        //check if valid
        if(unlockedAbilities == null || ability < 0 || ability >= unlockedAbilities.Length) { return false; }
        return unlockedAbilities[ability];
    }

    public AbilityTransfer PowerZero { get { return powerZero; } }
    public AbilityAbsorb PowerOne { get { return powerOne; } }
    public static bool[] Unlocked {
        get { return unlockedAbilities; }
        set { unlockedAbilities = value; }
    }
    public bool Inside { set { inside = value; } }

    // Use this for initialization 
    void Awake () {
        //find player refs
        player = Global.Player.GetComponent<Jump>();
    
        //find power zero refs 
        powerZero = Instantiate(abilityZeroPrefab).GetComponent<AbilityTransfer>();
        powerZero.gameObject.SetActive(false);
        part = GetComponent<ParticleSystem>();
        //part.Stop();
        particles = new ParticleSystem.Particle[1]; //array of length one for checks if part has ANY active particles

        //get the normal speeds as the origional values
        orignMoveSpeed = player.MoveSpeed;
        originJumpSpeed = player.JumpSpeed;

        partAbsorb = transform.GetChild(0).GetComponent<ParticleSystem>();
        partAbsorb.gameObject.SetActive(false);

        partWater = transform.GetChild(1).GetComponent<ParticleSystem>();
        partWater.gameObject.SetActive(true);

        powerOne = Instantiate(abilityOnePrefab).GetComponent<AbilityAbsorb>();
        powerOne.gameObject.SetActive(false);
    }

    void Start()
    {
        //all abilities start out false
        if (unlockedAbilities == null)
        {
            unlockedAbilities = new bool[4]; //SET number of abilities here
            LockAll();
            Unlock(0); //unlock the ability transfer powers
        }
        //setup has already occured in first scene
        else
        {
            if (unlockedAbilities[0])
            {
                powerZero.gameObject.SetActive(true);
            }
            if (unlockedAbilities[1] || unlockedAbilities[2])
            {
                partAbsorb.gameObject.SetActive(true);
                powerOne.gameObject.SetActive(true);
            }
            if (unlockedAbilities[3])
            {
                player.CanSwim = true;
            }
        }
    }

    /// <summary>
    /// set all the abilities to locked
    /// </summary>
    public void LockAll()
    {
        for (int i = 0; i < unlockedAbilities.Length; i++) { unlockedAbilities[i] = false; }
        player.CanSwim = false;
        powerZero.gameObject.SetActive(false);
        partAbsorb.gameObject.SetActive(false);
        powerOne.gameObject.SetActive(false);
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
                default:
                case 0:
                    powerZero.gameObject.SetActive(true);
                    break;
                case 1:
                case 2:
                    powerOne.gameObject.SetActive(true);
                    partAbsorb.gameObject.SetActive(true);
                    break;
                case 3:
                    player.CanSwim = true;
                    break;
            }
        }
    }

    void Update () {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.U))
        {
            for (int i = 0; i < unlockedAbilities.Length; i++) { Unlock(i); }
        }
#endif
        if (!Global.paused)
        {
            player.Power.SetOutlineColor(0);

            if (unlockedAbilities[0])
            {
                if (Heartbeat.BPM > 1 && !inside)
                {
                    powerZero.Useable = true;
                    //play the blled particle effect when mouse down or coming out of pause
                    if (Input.GetMouseButton(0) || Input.GetButton("Transfer") || (part.isPaused && part.GetParticles(particles) > 0))
                    {
                        part.Play();
                        powerZero.AudioPlayer.PlaySound("ContinueSparkle");
                        player.Power.SetOutlineColor(1);
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
                if ((Input.GetMouseButton(1) || Input.GetButton("Absorb")) && !inside)
                {
                    partAbsorb.Play();
                    player.Power.RestoreBPM(increaseRate * Time.deltaTime);
                    if(Heartbeat.BPM < 200) {
                        heartRateAdded += increaseRate * Time.deltaTime;
                    }
                    powerZero.AudioPlayer.PlaySound("ContinueSparkle");
                    player.Power.SetOutlineColor(2);
                }
                else
                {
                    partAbsorb.Stop();
                }
            }

            if (unlockedAbilities[2])
            {
                if ((Input.GetMouseButton(1) || Input.GetButton("Absorb")) && !inside)
                {
                    sprinting = true;
                    player.Heal(sprintHealRate * Time.deltaTime);
                }
                //stop the sprinting
                else if (sprinting && (Input.GetMouseButtonUp(1) || Input.GetButtonUp("Absorb")) && !inside)
                {
                    //player.Power.RemoveBPM(heartRateAdded + heartRateRemoved);
                    sprinting = false;
                    heartRateAdded = 0;
                }
                player.MoveSpeed = sprinting ? sprintMoveSpeed : orignMoveSpeed;
                player.JumpSpeed = sprinting ? sprintJumpSpeed : originJumpSpeed;
                player.Animator.speed = sprinting ? sprintMoveSpeed / orignMoveSpeed : 1; //set animation speed to match 
            }

            if (player.TouchingWater)
            {
                partWater.Play();
                partWater.Emit((int)(((player.GravityVelocity.magnitude * (1 - moveWeight)) +
                    (player.MoveVelocity.magnitude * moveWeight)) * rateOverDistance * Time.deltaTime));

                if (unlockedAbilities[3])
                {
                    player.Heal(waterHealRate * Time.deltaTime);
                    player.Power.SetOutlineColor(3);
                }
            }
            else
            {
                partWater.Stop();
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
