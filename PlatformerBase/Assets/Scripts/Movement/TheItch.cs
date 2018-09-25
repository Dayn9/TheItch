using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHealthObject))]
[RequireComponent(typeof(PhysicsObject))]
//FASTER HEALTH RATE DOES MORE DAMAGE
public class TheItch : MonoBehaviour {

    [SerializeField] private float timeStill; //time(seconds) the object can stand still before it takes damage
    [SerializeField] private int damageDelt; //damage the object takes when they stay still for too long
    private float timeObjectStill;  //time(seconds) the object has been still
    private const float movementBuffer = 0.01f; //min movement speed the player can move and not have heartrate slow

    private IHealthObject healthObject; //ref to attached Health Properties of Object
    private PhysicsObject physicsObject; //ref to attached Physics Properties of Object

    [SerializeField] private Heartbeat heartbeat; //ref to heartbeat controller script
    
    [SerializeField] [Range(0, 0.5f)]private float heartSpeed;
    private const float amp = 0.05f;
    private float period;
    private float rate;
    private float myBPM; 

    private void Awake()
    {
        healthObject = GetComponent<IHealthObject>();
        physicsObject = GetComponent<PhysicsObject>();

        period = heartbeat.MaxHR - heartbeat.RestingHR;
        myBPM = heartbeat.RestingHR;
    }

    void Update () {
        //object is standing still
        if(physicsObject.MoveVelocity.magnitude < movementBuffer)
        {
            if(myBPM <= heartbeat.RestingHR)
            {
                myBPM -= ((myBPM - (heartbeat.RestingHR - 20)) / 20) * heartSpeed * Time.deltaTime;  
                rate = 0;
            }
            else
            {
             
            }

        }
        //object is moving
        else
        {
            if(myBPM >= heartbeat.MaxHR)
            {
                myBPM -= ((myBPM - (heartbeat.MaxHR + 20)) / 20) * heartSpeed * Time.deltaTime;
                rate = 0;
            }
            else if(myBPM <= heartbeat.RestingHR)
            {
                rate = heartSpeed * Time.deltaTime;
                myBPM += rate;
            }
            else
            {
                rate = WaveFormula(myBPM);
                myBPM += (rate + heartSpeed) * Time.deltaTime;
            }
        }
        heartbeat.BPM = (int)myBPM;
        /*
        int modifiedBPM = Mathf.Clamp(heartbeat.BPM + (physicsObject.MoveVelocity.magnitude < movementBuffer ? -1 : 1), heartbeat.RestingHR, heartbeat.MaxHR);
        rate += (WaveFormula(modifiedBPM) * (physicsObject.MoveVelocity.magnitude < movementBuffer ? -1 : 1)) * Time.deltaTime / 2.5f;
        rate = Mathf.Clamp(rate, -2 * amp, 2 * amp);
        myBPM += rate;
        heartbeat.BPM = Mathf.Clamp((int)myBPM, heartbeat.RestingHR, heartbeat.MaxHR);
        */
        if (myBPM < heartbeat.RestingHR + 10.0f)
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

    private float WaveFormula(float x)
    {

        return (amp / 2) - ((amp / 2) * Mathf.Cos((2 * Mathf.PI * (x - heartbeat.RestingHR)) / period));
    }
}
