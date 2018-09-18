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
    private const float movementBuffer = 0.01f; //min movement speed the player can move and not take damage

    private IHealthObject healthObject; //ref to attached Health Properties of Object
    private PhysicsObject physicsObject; //ref to attached Physics Properties of Object

    private void Awake()
    {
        healthObject = GetComponent<IHealthObject>();
        physicsObject = GetComponent<PhysicsObject>();
    }

    void Update () {
        //chek if object is standing still
        if (physicsObject.MoveVelocity.magnitude < movementBuffer)
        {
            timeObjectStill += Time.deltaTime; //add to timer 
            //check if object has been standing still for too long
            if(timeObjectStill > timeStill)
            {
                healthObject.TakeDamage(damageDelt);
            }
        }
        //object is moving
        else
        {
            timeObjectStill = 0.0f;
        }
	}
}
