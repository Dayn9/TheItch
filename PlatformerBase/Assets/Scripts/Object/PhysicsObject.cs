using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PhysicsObject : MovingObject
{
    #region protected fields
    [SerializeField] protected bool inheritGravity; //inherit surface gravity on collision
    protected const float maxGravity = 2.0f;
    protected const float gravityMag = 1.0f; //constant magnitude of gravity
    protected static Vector2 gravity; //down direction
    protected Vector2 gravityVelocity = Vector2.zero; //initial velocity allways zero
    protected Vector2 groundNormal; //normal vector of the ground object is on
    protected Vector2 groundTangent; //Vector along ground
    protected bool grounded; //true if object is on the ground
    protected Rigidbody2D rb2D; //attached rigidbody
    protected ContactFilter2D filter; //collision filter
    protected SpriteRenderer sprite; //attachedd spriteRenderer
    protected Vector2 inputVelocity = Vector2.zero; //velocity form external forces 

    //Variables used in every collision check
    private float distance; //temporary distance to nearest collision
    private Vector2 moveVector; //temporary vector for collision checking
    private int numCollisions; //temporary number of collisions from collsion check
    protected RaycastHit2D[] hits; //temporary array of collisions
    private int layer; //temporary layer of collided object
    private Vector2 objectNormal; //temporary normal of solid colliding with

    protected bool touchingWater; //true when object is touching water
    protected bool stillTouchingWater;
    private const float waterMultiplier = 0.75f; 

    protected bool inFallZone; //true when the object is in a fallZone
    #endregion

    public Vector2 InputVelocity { set { inputVelocity = value;}}
    public Vector2 GravityVelocity { get { return gravityVelocity; } }
    public float MaxGravity { get { return maxGravity; } }
    public Vector2 Gravity { get { return gravity; } }
    public bool Grounded { get { return grounded; } }

    //Called on Initialization 
    protected virtual void Start()
    {
        gravity = Vector2.down * gravityMag; //default gravity vector is down
        groundNormal = -gravity.normalized;

        sprite = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.isKinematic = true; //make sure rb2D is kinematic

        hits = new RaycastHit2D[8];

        //get collision filter based on object layer
        filter.useTriggers = false;
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        filter.useLayerMask = true;
    }

    protected virtual void Update() {
        if (!paused)
        {
            if (!frozen)
            {
                moveVelocity = inputVelocity;
            }
            //object is frozen
            else
            {
                moveVelocity = Vector2.zero;
            }
           
        }
    }

    //called every Physics Update
    protected virtual void FixedUpdate()
    {
        if (!paused)
        {
            stillTouchingWater = false;
            #region gravity collision
            gravityVelocity += gravity * Time.deltaTime; //add gravity to velocity
            grounded = false;
            if (!inFallZone)
            {
                moveVector = gravityVelocity; //temporary falling vector for collision checking
                if (touchingWater) { moveVector *= waterMultiplier; } //scale down vector when in water
                distance = Mathf.Clamp(moveVector.magnitude, 0, maxGravity); //temporary distance to surface
                Vector2 newGroundNormal = groundNormal; //temporary normal for surface collisions
                int numCollisions = rb2D.Cast(moveVector, filter, hits, distance);
                for (int i = 0; i < numCollisions; i++)
                {
                    Vector2 objectNormal = hits[i].normal; //get the normal vector of the surface

                    //check not collision inside an object or into wall
                    if (hits[i].distance != 0 && Vector2.Dot(gravity, objectNormal) != 0)
                    {
                        if (Vector2.Dot(gravityVelocity, gravity) >= 0) { grounded = true; } //check if velocity is in the same direction as gravity (falling)

                        //gravityVelocity = Vector2.zero; //stop moving * * * MOVED TO COLLIDE WITH CLOSEST SECTION * * *

                        float moveableDistance = moveVector.magnitude;
                        if (LayerChecks(hits[i].transform.gameObject, moveVector.normalized * (moveableDistance - buffer), out moveableDistance))
                        {
                            //collide with the closest
                            if (moveableDistance < distance)
                            {
                                newGroundNormal = objectNormal; //set new ground normal
                                distance = moveableDistance;
                            }
                        }
                        //collide with the closest
                        else if (hits[i].distance < distance)
                        {
                            gravityVelocity = Vector2.zero; //stop moving

                            distance = hits[i].distance; //set new closest distance
                            newGroundNormal = objectNormal; //set new ground normal

                            if (inheritGravity)
                            {
                                gravity = -objectNormal * gravity.magnitude; //inherit gavity of new surface
                                rb2D.rotation += Vector2.SignedAngle(-transform.up, gravity); //match rotation
                            }
                        }
                    }
                }
                rb2D.position += moveVector.normalized * (distance - buffer); //move object by the distance to nearest collision
                groundNormal = newGroundNormal; //set the ground normal to normal of closest surface
            }
            #endregion

            #region movement collision
            groundTangent = grounded ? Tangent(groundNormal) : Tangent(-gravity); //set the ground Tangent
            moveVector = Proj(moveVelocity, groundTangent); //Project the moveVelocity onto the ground
            if (touchingWater) { moveVector *= waterMultiplier; } //scale down vector when in water
            distance = moveVector.magnitude; //temporary distance to surface
            numCollisions = rb2D.Cast(moveVector, filter, hits, distance);
            for (int i = 0; i < numCollisions; i++)
            {
                float moveableDistance = moveVector.magnitude;
                if (LayerChecks(hits[i].transform.gameObject, moveVector.normalized * (moveableDistance - buffer), out moveableDistance))
                {
                    //collide with the closest
                    if (moveableDistance < distance)
                    {
                        distance = moveableDistance;
                    }
                }
                //check not collision inside an object 
                else if (hits[i].distance != 0)
                {
                    //collide with the closest 
                    if (hits[i].distance <= distance)
                    {
                        distance = hits[i].distance; //set new closest distance
                    }
                }
            }

            if (distance > buffer) { rb2D.position += moveVector.normalized * (distance - buffer); } //move object by the distance to nearest collision
            if (moveVector.magnitude > buffer) { sprite.flipX = Vector2.Dot(transform.right, moveVector) < 0; } //face the correct direction
            #endregion
            touchingWater = stillTouchingWater; 
        }

    }

    /// <summary>
    /// Checks collision allong specified velocity
    /// </summary>
    /// <param name="velocityType"></param>
    public float InputCollision(bool grounded)
    {
        //initial variable use for each velocity type
        float distance = inputVelocity.magnitude; //temporary distance to surface       
        groundTangent = grounded ? Tangent(groundNormal) : Tangent(-gravity); //set the ground Tangent
        Vector2 moveVector = grounded ? Proj(inputVelocity, groundTangent): inputVelocity; //Project the moveVelocity onto the ground

        RaycastHit2D[] hits = new RaycastHit2D[8]; //create a new hits array for storing collision data

        float numCollisions = rb2D.Cast(moveVector, filter, hits, distance); //cast the rigidbody into the scene and get collisions in hits
        for (int i = 0; i < numCollisions; i++)
        {
            //collide with the closest not inside
            if (hits[i].distance != 0 && hits[i].distance <= distance)
            {
                distance = hits[i].distance; //set new closest distance
            }
        }
        if (distance > buffer) { rb2D.position += moveVector.normalized * (distance - buffer); } //move object by the distance to nearest collision
        return distance - buffer; //return the total distance moved
    }

    #region helper methods
    /// <summary>
    /// handles additional layer checks for specific collision behavior
    /// </summary>
    /// <param name="layer">layer collided with</param>
    private bool LayerChecks(GameObject collided, Vector2 moveVector, out float distance)
    {
        distance = moveVector.magnitude;
        switch (collided.layer)
        {
            //take damage when colliding with spikes
            case 13: //Spikes
                HitSpikes();
                break;
            //Move any objects that can be moved
            case 10:
                //transform.parent = collided.transform;
                MovingObject moveingObj = collided.GetComponent<MovingObject>();
                if (!moveingObj) { moveingObj = collided.GetComponentInParent<MovingObject>(); }
                if (moveingObj != null)
                {
                    inputVelocity = moveingObj.MoveVelocity ; //get the input Veclocity                    
                    inputVelocity -= Vector2.Dot(inputVelocity, gravity) * (gravity * Time.deltaTime);
                    //inherit gravity
                    if (Vector2.Dot(inputVelocity, gravity) != 0)
                    {
                        gravityVelocity = inputVelocity;
                    }
                    InputCollision(grounded);

                    grounded = true; //insure grounded 
                }
                break;

            case 11: //SolidMovableObject
                PhysicsObject moveObj = collided.GetComponent<PhysicsObject>();
                if (moveObj != null)
                {
                    moveObj.InputVelocity = moveVector;
                    distance =  moveObj.InputCollision(grounded); //move the object and return the ditance it moved
                    return true;
                }
                break;
            case 14: //Ladder
                TouchLadder();
                return true; //don't collide with ladder 
            case 4: //Water
                TouchWater();
                return true;
        }
        return false;
    }

    /// <summary>
    /// change the Physics 2D colision filter between this object layer and another
    /// </summary>
    /// <param name="layer">collision layer of other object</param>
    /// <param name="collide">should this object be able to collide with the other</param>
    public void SetCollision(string layer, bool collide)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer(layer), collide);
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    /// <summary>
    /// to be overridden in child classes when object collides with ladders
    /// </summary>
    protected virtual void TouchLadder() { }

    /// <summary>
    /// to be overridden in child classes when object collides with ladders
    /// </summary>
    protected virtual void TouchWater()
    {
        stillTouchingWater = true;
        touchingWater = true;
    }

    /// <summary>
    /// to be overridden in child classes when object collides with spikes
    /// </summary>
    protected virtual void HitSpikes() { }

    /// <summary>
    /// get projection of one vector onto another
    /// </summary>
    /// <param name="v1">origional vector</param>
    /// <param name="v2">vector projecting onto</param>
    /// <returns></returns>
    protected Vector2 Proj(Vector2 v1, Vector2 v2)
    {
        return (Vector2.Dot(v1, v2) / Mathf.Pow(v2.magnitude, 2)) * v2; //projection formula
    }

    /// <summary>
    /// rotate vector 90 degrees
    /// </summary>
    /// <param name="normal">origional vector</param>
    /// <returns>orthogonal vector</returns>
    protected Vector2 Tangent(Vector2 normal)
    {
        return new Vector2(normal.y, -normal.x); //apply and return rotation
    }
    #endregion
}