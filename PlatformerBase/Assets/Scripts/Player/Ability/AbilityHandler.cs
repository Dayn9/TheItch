using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AbilityHandler : Global {

    /// <summary>
    /// in charge of the different abilities the player has
    /// </summary>

    private IPlayer player;
    private Heartbeat hb; //ref to the heartbeat component 

    [Header("Ability 0: Transfer")]
    [SerializeField] private GameObject abilityZeroPrefab; //prefab of the first bleed ability
    private ParticleSystem part; //particle system attached to object
    protected ParticleSystem.Particle[] particles; //array of particles being controlled 
    private AbilityTransfer powerZero; //ref to the abilityTransfer Compnent of the First ability
    public AbilityTransfer PowerZero { get { return powerZero; } }

    [Header("Ability 1: Sprinting")]
    private float normalMoveSpeed;
    private float normalJumpSpeed;
    [SerializeField] private float sprintMoveSpeed; //movement speed while sprinting
    [SerializeField] private float sprintJumpSpeed; //jump speed while sprinting
    [SerializeField] private float exhaustMoveSpeed; //movement speed after sprinting
    [SerializeField] private float exhaustJumpSpeed; //jump speed after sprinting
    private bool sprinting = false; //true when the player is sprinting
    private const int minSprintBPM = 170; //minimum heartrate the player can use the sprint ability at
     
    //idea for exhaust: have a sprint timer that increases while sprinting and then decreases while exhasted

    private static bool[] unlockedAbilities; //array for which abilities have been unlocked

    // Use this for initialization
    void Awake () {
        //find player refs
        player = Player.GetComponent<IPlayer>();
        hb = player.Power.Heartbeat;

        //get the normal speeds as the origional values
        normalMoveSpeed = player.MoveSpeed;
        normalJumpSpeed = player.JumpSpeed;

        //find power zero refs 
        powerZero = Instantiate(abilityZeroPrefab).GetComponent<AbilityTransfer>();
        powerZero.gameObject.SetActive(false);
        part = GetComponent<ParticleSystem>();
        //part.Stop();
        particles = new ParticleSystem.Particle[1]; //array of length one for checks if part has ANY active particles

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
        }
    }

    /// <summary>
    /// set all the abilities to locked
    /// </summary>
    public void LockAll()
    {
        for (int i = 0; i < unlockedAbilities.Length; i++) { unlockedAbilities[i] = false; }
        Unlock(0); //unlock the ability transfer powers
        Unlock(1); //unlock the sprint ability
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
            if (unlockedAbilities[1])
            {
                sprinting = Input.GetButton("Sprint");
                player.MoveSpeed = sprinting ? sprintMoveSpeed : normalMoveSpeed;
                player.JumpSpeed = sprinting ? sprintJumpSpeed : normalJumpSpeed;
                player.Animator.speed = sprinting ? sprintMoveSpeed / normalMoveSpeed : 1; //set animation speed to match 
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
