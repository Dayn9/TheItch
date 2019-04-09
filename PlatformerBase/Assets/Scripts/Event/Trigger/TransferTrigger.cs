using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHealthObject))]
public class TransferTrigger : IndicatorTrigger, ILevelData
{
    private IHealthObject healthObj;

    private const float transferRate = 15.0f;
    private bool transfering = false;
    private bool absorbing = false;
    public HeartbeatIndicator HbIndicator { get; private set; }

    private Animator anim;

    //clicking related variables
    private BoxCollider2D zone;
    private bool validInput = false;

    [SerializeField] private Sprite inHighlight;
    [SerializeField] private Sprite outHighlight;

    [SerializeField] private Texture2D cursor;
    [SerializeField] private Texture2D cursorH;

    private bool containsMouse = false;

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

        HbIndicator.Total = healthObj.MaxHealth;
        HbIndicator.CurrentHealth = healthObj.Health;

        zone = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("full", FullyHealed);
    }

    private void Start()
    {
        abilityHandler = Player.GetComponentInChildren<AbilityHandler>();
    }

    /// <summary>
    /// returns true when the attached heart Object is fully healed
    /// </summary>
    public bool FullyHealed { get { return healthObj.Health == healthObj.MaxHealth; } }
    public bool Empty { get { return healthObj.Health == 0; } }

    public bool State { get { return FullyHealed; } }
    public void OnLevelLoad(bool state)
    {
        if (state) { healthObj.FullHeal(); }
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
        }
    }

    private void GetMouseClick()
    {
        validInput = ((Empty && Heartbeat.BPM > healthObj.MaxHealth)
                || (FullyHealed && AbilityHandler.IsUnlocked(1))); //check if indicator should be available

        //check for mouse in zone and able to interact
        if (zone.bounds.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)) && validInput)
        {
            indicator.SetActive(true);

            if (!containsMouse) {
                Cursor.SetCursor(cursorH, Vector2.zero, CursorMode.Auto);
                audioPlayer.PlaySound(0); //play the hover sound
            }

            if (Input.GetMouseButton(0) && !transfering && !FullyHealed)
            {
                transfering = true;
                Player.GetComponent<IPlayer>().Power.RemoveBPM(healthObj.MaxHealth);

                Player.GetComponent<IHealthObject>().Damage(0); //triggers the damage animation
                CallBefore();

                abilityHandler.PowerZero.SendParticlesTo(transform, healthObj.MaxHealth);
            }
            else if(Input.GetMouseButton(1) && !absorbing && !Empty)
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
                Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
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
