using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHealthObject))]
[RequireComponent(typeof(PhysicsObject))]
//FASTER HEALTH RATE DOES MORE DAMAGE
public class TheItch : Global
{

    [SerializeField] private float timeStill; //time(seconds) the object can stand still before it takes damage
    [SerializeField] private int damageDelt; //damage the object takes when they stay still for too long
    private float timeObjectStill;  //time(seconds) the object has been still
    private const float movementBuffer = 0.01f; //min movement speed the player can move and not have heartrate slow

    private IHealthObject healthObject; //ref to attached Health Properties of Object
    private PhysicsObject physicsObject; //ref to attached Physics Properties of Object

    [SerializeField] private Heartbeat heartbeat; //ref to heartbeat controller script
    [SerializeField] private float minHeartSpeed;
    [SerializeField] private float maxHeartSpeed;
    private float heartPosition;
    private float heartSpeed = 0.0f;
    [SerializeField] private float heartAccelerate;
    private float heartDrag;

    private void Awake()
    {
        healthObject = GetComponent<IHealthObject>();
        physicsObject = GetComponent<PhysicsObject>();
    }

    private void Start()
    {
        heartPosition = heartbeat.BPM;
    }

    void Update()
    {
        if (!paused)
        {
            //object is standing still
            if (physicsObject.MoveVelocity.magnitude < movementBuffer)
            {
                if (heartPosition <= heartbeat.RestingHR)
                {
                    heartSpeed = (heartPosition - (heartbeat.RestingHR - 20)) * -minHeartSpeed * Time.deltaTime;
                    heartDrag = 0;
                }
                else
                {
                    heartSpeed -= heartAccelerate * Time.deltaTime;
                    // increase the drag force when speed is positive 
                    heartDrag = heartSpeed > 0 ? Mathf.Pow(heartSpeed, 2) / 1.5f : Mathf.Pow(heartSpeed, 2) / 2.0f;
                }
            }
            //object is moving
            else
            {
                if (heartPosition >= heartbeat.MaxHR)
                {
                    heartSpeed = ((heartPosition - (heartbeat.MaxHR + 20)) / 20) * minHeartSpeed * Time.deltaTime;

                }
                else
                {
                    heartSpeed += heartAccelerate * Time.deltaTime;
                    //increase the drag force when speed is negative
                    heartDrag = heartSpeed < 0 ? Mathf.Pow(heartSpeed, 2) / 1.5f : Mathf.Pow(heartSpeed, 2) / 2.0f;

                }
            }

            heartSpeed = Mathf.Clamp(heartSpeed, -maxHeartSpeed, maxHeartSpeed);
            heartPosition += (heartSpeed - heartDrag) * 0.1f;
            heartbeat.BPM = (int)heartPosition;
            /*
            int modifiedBPM = Mathf.Clamp(heartbeat.BPM + (physicsObject.MoveVelocity.magnitude < movementBuffer ? -1 : 1), heartbeat.RestingHR, heartbeat.MaxHR);
            rate += (WaveFormula(modifiedBPM) * (physicsObject.MoveVelocity.magnitude < movementBuffer ? -1 : 1)) * Time.deltaTime / 2.5f;
            rate = Mathf.Clamp(rate, -2 * amp, 2 * amp);
            myBPM += rate;
            heartbeat.BPM = Mathf.Clamp((int)myBPM, heartbeat.RestingHR, heartbeat.MaxHR);
            */
            if (heartPosition <= heartbeat.RestingHR)
            {
                timeObjectStill += Time.deltaTime; //add to timer 
                                                   //check if object has been standing still for too long
                if (timeObjectStill > timeStill)
                {
                    healthObject.TakeDamage(damageDelt);
                }
            }
            else
            {
                timeObjectStill = 0.0f;
            }
        }
    }
}
