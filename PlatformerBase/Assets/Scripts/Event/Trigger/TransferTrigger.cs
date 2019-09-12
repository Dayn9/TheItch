using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(IHealthObject))]
public class TransferTrigger : IndicatorTrigger
{
    private IHealthObject healthObj;

    private const float transferRate = 15.0f;
    private bool transfering = false;
    private bool absorbing = false;
    public HeartbeatIndicator HbIndicator { get; private set; }

    private Animator anim;
    private SpriteRenderer render;

    //clicking related variables
    private BoxCollider2D zone;
    private bool validInput = false;

    [SerializeField] private Sprite inHighlight;
    [SerializeField] private Sprite outHighlight;

    private bool containsMouse = false;

    private static CursorChange cursorChange;
    private static AbilityHandler abilityHandler;

    protected override void Awake()
    {
        base.Awake();
        healthObj = GetComponent<IHealthObject>();

        //make sure the indicator has a heartbeat indicator
        if ((HbIndicator = indicator.GetComponent<HeartbeatIndicator>()) == null)
        {
            HbIndicator = indicator.AddComponent<HeartbeatIndicator>();
        }
        zone = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }


    private void Start()
    {
        abilityHandler = Player.GetComponentInChildren<AbilityHandler>();
        cursorChange = MainCamera.GetComponent<CursorChange>();
        HbIndicator.Total = healthObj.MaxHealth;
        HbIndicator.CurrentHealth = healthObj.Health;
        anim.SetBool("full", FullyHealed);
    }

    /// <summary>
    /// returns true when the attached heart Object is fully healed
    /// </summary>
    public bool FullyHealed { get { return healthObj.Health == healthObj.MaxHealth; } }
    public bool Empty { get { return healthObj.Health == 0; } }

    public override bool State { get { return FullyHealed; } }
    public override void OnLevelLoad(bool state)
    {
        healthObj = GetComponent<IHealthObject>();
        if (state) {
            healthObj.FullHeal();
        }
        else
        {
            healthObj.Damage(healthObj.MaxHealth);
        }
    }

    protected override void Update()
    {
        if (!paused)
        {
            GetMouseClick();

            if (transfering)
            {
                indicator.SetActive(true);
                if (FullyHealed)
                {
                    //CallAfter();
                    transfering = false;                    
                }
                else
                {
                    healthObj.Heal(transferRate * Time.deltaTime);
                }
                //update the heartbeatIndicator
                HbIndicator.CurrentHealth = healthObj.Health;
            }
            else if (absorbing)
            {
                indicator.SetActive(true);
                if (Empty)
                {
                    absorbing = false;
                }
                else
                {
                    healthObj.Damage(transferRate * Time.deltaTime);
                }
                //update the heartbeatIndicator
                HbIndicator.CurrentHealth = healthObj.Health;
            }


            //set animation
            anim.SetBool("full", FullyHealed);
            if (containsMouse)
            {
                HbIndicator.SetSprite(anim.GetBool("full") ? outHighlight : inHighlight);
            }
            //fade the color when ability one hasn't been  unlocked yet
            if((FullyHealed && !AbilityHandler.IsUnlocked(1)) || (!FullyHealed && !AbilityHandler.IsUnlocked(0)))
            {
                render.color = Color.Lerp(new Color(1, 1, 1, 0.25f), render.color, 0.9f);
            }
            else
            {
                render.color = Color.Lerp(Color.white, render.color, 0.9f);
            }

        }
    }

    private void GetMouseClick()
    {
        validInput = ((Empty && Heartbeat.BPM > healthObj.MaxHealth && AbilityHandler.IsUnlocked(0))
                || (FullyHealed && AbilityHandler.IsUnlocked(1))); //check if indicator should be available

        //check for mouse in zone and able to interact
        if (((!JoystickMouse.Active && zone.bounds.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)))
           || (JoystickMouse.Active && zone.bounds.Contains(JoystickMouse.Pos)))
            && validInput)
        {
            indicator.SetActive(true);

            if (!containsMouse) {
                cursorChange.Hovering = true;
                audioPlayer.PlaySound(0); //play the hover sound
            }

            if ((Input.GetMouseButton(0) || Input.GetButton("Transfer")) && !transfering && !FullyHealed)
            {
                transfering = true;
                Player.GetComponent<IPlayer>().Power.RemoveBPM(healthObj.MaxHealth);

                Player.GetComponent<IHealthObject>().Damage(0); //triggers the damage animation
                CallBefore();

                abilityHandler.PowerZero.SendParticlesTo(transform, healthObj.MaxHealth);
            }
            else if((Input.GetMouseButton(1) || Input.GetButton("Absorb")) && !absorbing && !Empty)
            {
                absorbing = true;
                Player.GetComponent<IPlayer>().Power.RestoreBPM(healthObj.MaxHealth);
                CallAfter();

                abilityHandler.PowerOne.transform.position = transform.position;
                abilityHandler.PowerOne.SendParticlesTo(Player.GetComponent<MovingObject>(), healthObj.MaxHealth);
                audioPlayer.PlaySound(1);
            }
            containsMouse = true;
        }
        //unable to interact
        else {
            //GetComponent<SpriteRenderer>().sprite = fullHighlight;
            indicator.SetActive(false);
            if (containsMouse)
            {
                cursorChange.Hovering = false;
                containsMouse = false;
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player") //trigger sound when player enters 
        {
            if (!containsMouse && validInput) { audioPlayer.PlaySound(0); }
        }
    }

    protected override void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "AbilityOne") //exit dialogue when player leaves
        {
            //indicator.SetActive(false);
            //playerTouching = false;
            if (disableAfter) { gameObject.SetActive(false); }
        }
    }
}
