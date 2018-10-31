using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadCamera : BaseCamera {

    [SerializeField] private PhysicsObject follow; //object to follow and move base on it's Move velociity
    [SerializeField] private float leaveTime; //time it takes to move away from follow object
    [SerializeField] private float returnTime; //time it taks to move back to follow object
    [SerializeField] private float dist; //maximum distance from follow object to move to
    private Vector2 smoothVel = Vector2.zero; //velocity of the camera

    [SerializeField] private Vector2 offsetFromPlayer;
    private Vector2 currentOffset = Vector2.zero; //offset from follow's position
    private Vector2 targetOffset = Vector2.zero; //target offset from follow's current position
    private Vector3 newOffset; //temporary calculated offset to move to

    private void Awake()
    {
        if(follow == null) { follow = GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsObject>(); }
    }

    void Update()
    {
        if (!paused)
        {
            targetOffset = (offsetFromPlayer * (follow.GravityVelocity.y < 0 ? -1 : 1 ))
                + (follow.MoveVelocity.normalized * dist); //target offset determined direction of MoveVelocity and magnitude of dist

            //calculate new offset with speed depending on if target is towards or away from follow
            newOffset = Vector2.SmoothDamp(currentOffset, targetOffset, ref smoothVel,
                targetOffset.magnitude > buffer ? leaveTime : returnTime);

            newOffset.z = transform.position.z; //maintain z position
            transform.position = follow.transform.position + newOffset; //move to new Offset from follow
            currentOffset = newOffset; //update the current offset

            StayInLimits(); //remain within the limits of the level
        }
    }
}
