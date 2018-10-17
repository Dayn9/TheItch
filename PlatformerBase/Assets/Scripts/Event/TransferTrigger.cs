using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHealthObject))]
public class TransferTrigger : EventTrigger
{
    private IHealthObject healthObj;

    private const float transferRate = 5.0f;
    private float heartbeatToTransfer;

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

    public bool FullyHealed { get { return healthObj.Health == healthObj.MaxHealth; } }

    protected override void Update()
    {
        if (!paused)
        {
            //check if in contact with the player and player is interacting 
            if (playerTouching && Input.GetKey(trigger))
            {   
                if (questCompleted && FullyHealed)
                {
                    CallAfter();
                    Debug.Log(FullyHealed);
                }
                else
                {
                    if (Input.GetKeyDown(trigger)) { CallBefore(); }

                    heartbeatToTransfer = transferRate * Time.deltaTime;
                    Player.GetComponent<IPlayer>().Power.RemoveBPM(heartbeatToTransfer);
                    healthObj.Heal(heartbeatToTransfer);
                }
                //update the heartbeatIndicator
                hbIndicator.CurrentHealth = healthObj.Health;
            }
        }
    }
}
