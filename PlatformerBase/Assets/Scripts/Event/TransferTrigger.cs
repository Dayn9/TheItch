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

    private HeartbeatIndicator hbIndicator;
    public HeartbeatIndicator HbIndicator { get { return hbIndicator; } }

    //clicking related variables
    private BoxCollider2D zone;
    private bool active = false;

    [SerializeField] private Sprite mouseOffSprite;
    [SerializeField] private Sprite mouseOnSprite;

    [SerializeField] private Texture2D cursor;
    [SerializeField] private Texture2D cursorH;

    private bool containsMouse = false;

    private void Start()
    {
        healthObj = GetComponent<IHealthObject>();
        //make sure the indicator has a heartbeat indicator
        if ((hbIndicator = indicator.GetComponent<HeartbeatIndicator>()) == null)
        {
            hbIndicator = indicator.AddComponent<HeartbeatIndicator>();
        }

        hbIndicator.Total = healthObj.MaxHealth;
        hbIndicator.CurrentHealth = 0;

        zone = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// returns true when the attached heart Object is fully healed
    /// </summary>
    public bool FullyHealed { get { return healthObj.Health == healthObj.MaxHealth; } }

    protected override void Update()
    {
        if (!paused)
        {
            active = transfering || ((Input.GetKey(KeyCode.X) || Input.GetMouseButton(0)) && Player.GetComponent<IPlayer>().Power.Heartbeat.BPM > healthObj.MaxHealth);

            GetMouseClick();

            if (transfering)
            {
                indicator.SetActive(true);
                Player.GetComponent<IPlayer>().Power.SetDamageColor(true);
                if (FullyHealed)
                {
                    Player.GetComponent<IPlayer>().Power.SetDamageColor(false);
                    CallAfter();
                    transfering = false;
                }
                else
                {
                    heartbeatToTransfer = transferRate * Time.deltaTime;
                    healthObj.Heal(heartbeatToTransfer);                    
                }
                //update the heartbeatIndicator
                hbIndicator.CurrentHealth = healthObj.Health;
            }
        }
    }

    private void GetMouseClick()
    {
        if (zone.bounds.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)))
        {
            GetComponent<SpriteRenderer>().sprite = mouseOnSprite;
            indicator.SetActive(true);

            if (!containsMouse) {
                Cursor.SetCursor(cursorH, Vector2.zero, CursorMode.Auto);
                audioPlayer.PlaySound(0); //play the hover sound
            } 

            if (active && !transfering && !FullyHealed)
            {
                transfering = true;
                Player.GetComponent<IPlayer>().Power.RemoveBPM(healthObj.MaxHealth);
                Player.GetComponent<IPlayer>().Power.SetDamageColor(true);

                Player.GetComponent<IHealthObject>().TakeDamage(0); //triggers the damage animation
                CallBefore();

                Player.GetComponentInChildren<AbilityHandler>().PowerOne.SendParticlesTo(transform, healthObj.MaxHealth);
            }
            containsMouse = true;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = mouseOffSprite;
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
        }
    }
}
