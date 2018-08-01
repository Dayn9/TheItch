using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FollowPlayer : PhysicsObject {

    [SerializeField] private float moveSpeed; //how fast the object can move
    [SerializeField] private float minDistanceToPlayer; //distance at which the object will stop moving towards the player
    [SerializeField] private Transform player; //ref to transform component of player object 

    private bool rightLedge = true; //true when there is a ledge to the right of object
    private bool leftLedge = true; //true when there is a ledge to the left of object

    private float distanceToPlayer; //distance from object to player

    private void Awake()
    {
        //add Ledge Detected Method to events of ledge detectors
        EdgeDetector[] detectors = GetComponentsInChildren<EdgeDetector>();
        foreach(EdgeDetector detector in detectors)
        {
            detector.LedgeDetected += OnLedgeDetect;
        }
    }

    private void Update()
    {
        moveVelocity = Vector2.zero; //reset moveVelocity every update
        distanceToPlayer = transform.position.x - player.position.x; //find distance to player
        //move towards player horizontally
        if (Mathf.Abs(distanceToPlayer) > minDistanceToPlayer)
        {
            float sign = Mathf.Sign(distanceToPlayer);
            if((sign < 0 && !rightLedge) || (sign > 0 && !leftLedge))
            {
                moveVelocity = Vector2.left * sign * moveSpeed * Time.deltaTime;
            }
        }
    }

    private void OnLedgeDetect(bool right)
    {
        //ledge to right
        if (right) { rightLedge = !rightLedge; }
        //ledge to left
        else { leftLedge = !leftLedge; }
    }
}
