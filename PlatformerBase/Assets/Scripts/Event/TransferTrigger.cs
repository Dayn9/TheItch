﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHealthObject))]
public class TransferTrigger : IndicatorTrigger
{
    private IHealthObject healthObj;

    private const float transferRate = 5.0f;
    private float heartbeatToTransfer;
    private bool transfering = false;

    private HeartbeatIndicator hbIndicator;

    public HeartbeatIndicator HbIndicator { get { return hbIndicator; } }
    private void Start()
    {
        healthObj = GetComponent<IHealthObject>();
        //make sure the indicator has a heartbeat indicator
        if(indicator.GetComponent<HeartbeatIndicator>() == null)
        {
            indicator.AddComponent<HeartbeatIndicator>();
        }
        hbIndicator = indicator.GetComponent<HeartbeatIndicator>();

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
            //check if transfering but shouldn't be
            if(transfering && (!playerTouching || FullyHealed))
            {
                transfering = false;
                Player.GetComponent<IPlayer>().Power.SetDamageColor(false);
                questCompleted = true; //
            }
            //check if in contact with the player and player is interacting 
            else if (playerTouching && (Input.GetKeyDown(triggers[0]) || Input.GetKeyDown(triggers[1])))
            {   
                if (questCompleted && FullyHealed)
                {
                    CallAfter();
                }
                else
                {
                    transfering = true;
                }
            }

            //transfer heartrate from player to heartrate object
            if (transfering)
            {
                heartbeatToTransfer = transferRate * Time.deltaTime;
                Player.GetComponent<IPlayer>().Power.RemoveBPM(heartbeatToTransfer);
                Player.GetComponent<IPlayer>().Power.SetDamageColor(true);

                Player.GetComponent<IHealthObject>().TakeDamage(0); //triggers the damage animation
                healthObj.Heal(heartbeatToTransfer);

                CallBefore();
            }
            //update the heartbeatIndicator
            hbIndicator.CurrentHealth = healthObj.Health;
        }
    }
}
