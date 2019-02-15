using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHealthObject))]
public class TransferTrigger : IndicatorTrigger
{
    private IHealthObject healthObj;

    private const float transferRate = 15.0f;
    private float heartbeatToTransfer;
    private bool transfering = false;
    public HeartbeatIndicator HbIndicator { get; private set; }

    private Animator anim;

    //clicking related variables
    private BoxCollider2D zone;
    private bool active = false;

    [SerializeField] private Sprite inHighlight;
    [SerializeField] private Sprite outHighlight;

    [SerializeField] private Texture2D cursor;
    [SerializeField] private Texture2D cursorH;

    private bool containsMouse = false;


    private void Start()
    {
        healthObj = GetComponent<IHealthObject>();
        //make sure the indicator has a heartbeat indicator
        if ((HbIndicator = indicator.GetComponent<HeartbeatIndicator>()) == null)
        {
            HbIndicator = indicator.AddComponent<HeartbeatIndicator>();
        }

        HbIndicator.Total = healthObj.MaxHealth;
        HbIndicator.CurrentHealth = 0;

        zone = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// returns true when the attached heart Object is fully healed
    /// </summary>
    public bool FullyHealed { get { return healthObj.Health == healthObj.MaxHealth; } }

    protected override void Update()
    {
        if (!paused)
        {
            active = transfering || (Input.GetMouseButton(0) && Player.GetComponent<IPlayer>().Power.Heartbeat.BPM > healthObj.MaxHealth);

            GetMouseClick();


            if (transfering)
            {
                indicator.SetActive(true);
                if (FullyHealed)
                {
                    CallAfter();
                    transfering = false;
                }
                else
                {
                    heartbeatToTransfer = transferRate * Time.deltaTime;
                    healthObj.Heal(heartbeatToTransfer);                    
                }
                //update the heartbeatIndicator
                HbIndicator.CurrentHealth = healthObj.Health;
            }



            //set animation
            anim.SetBool("full", FullyHealed);
            if (containsMouse)
            {
                HbIndicator.SetSprite(FullyHealed ? outHighlight : inHighlight);
            }
        }
    }

    private void GetMouseClick()
    {
        if (zone.bounds.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)))
        {
            indicator.SetActive(true);

            if (!containsMouse) {
                Cursor.SetCursor(cursorH, Vector2.zero, CursorMode.Auto);
                audioPlayer.PlaySound(0); //play the hover sound
            } 

            if (active && !transfering && !FullyHealed)
            {
                transfering = true;
                Player.GetComponent<IPlayer>().Power.RemoveBPM(healthObj.MaxHealth);

                Player.GetComponent<IHealthObject>().TakeDamage(0); //triggers the damage animation
                CallBefore();

                Player.GetComponentInChildren<AbilityHandler>().PowerZero.SendParticlesTo(transform, healthObj.MaxHealth);
            }
            containsMouse = true;
        }
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
            if (!containsMouse) { audioPlayer.PlaySound(0); }
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
