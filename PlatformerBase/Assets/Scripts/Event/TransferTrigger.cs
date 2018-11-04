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
    }

    /// <summary>
    /// returns true when the attached heart Object is fully healed
    /// </summary>
    public bool FullyHealed { get { return healthObj.Health == healthObj.MaxHealth; } }

    protected override void Update()
    {
        if (!paused)
        {
            //indicator is active when transfering can happen
            indicator.SetActive(transfering || (Input.GetKey(KeyCode.X) || Input.GetMouseButton(0)));

            if (transfering)
            {
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

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "AbilityOne") //trigger dialogue when player touches 
        {
            //indicator.SetActive(true);

            if(!transfering && !FullyHealed)
            {
                transfering = true;
                Player.GetComponent<IPlayer>().Power.RemoveBPM(healthObj.MaxHealth);
                Player.GetComponent<IPlayer>().Power.SetDamageColor(true);

                Player.GetComponent<IHealthObject>().TakeDamage(0); //triggers the damage animation
                CallBefore();

                coll.GetComponentInParent<AbilityTransfer>().SendParticlesTo(transform.position, healthObj.MaxHealth);
            }
            //playerTouching = true;
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
