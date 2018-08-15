using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PhysicsObject : Inventory
{
    #region protected fields
    [SerializeField] protected bool inheritGravity; //inherit surface gravity on collision

    protected const float gravityMag = 1.0f; //constant magnitude of gravity
    protected static Vector2 gravity; //down direction

    protected Vector2 velocity = Vector2.zero; //initial velocity allways zero
    protected Vector2 groundNormal; //normal vector of the ground object is on
    protected Vector2 groundTangent; //Vector along ground
    protected bool grounded; //true if object is on the ground

    protected Rigidbody2D rb2D; //attached rigidbody
    protected RaycastHit2D[] hits; //array of collisions
    protected ContactFilter2D filter; //collision filter

    protected SpriteRenderer sprite; //attachedd spriteRenderer

    protected Vector2 moveVelocity = Vector2.zero; //initial moving Velocity

    #endregion 

    // Use this for initialization
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

    protected virtual void FixedUpdate()
    {
        velocity += gravity * Time.deltaTime; //add gravity to velocity
        grounded = false;

        #region gravity collision
        float distance = velocity.magnitude; //temporary distance to surface
        Vector2 newGroundNormal = groundNormal; //temporary normal for surface collisions
        Vector2 moveVector = velocity * Time.deltaTime; //temporary falling vector for collision checking

        int numCollisions = rb2D.Cast(moveVector, filter, hits, distance);
        for (int i = 0; i < numCollisions; i++)
        {
            Vector2 objectNormal = hits[i].normal; //get the normal vector of the surface

            //check not collision inside an object or into wall
            if (hits[i].distance != 0 && Vector2.Dot(gravity, objectNormal) != 0)
            {
                if (Vector2.Dot(velocity, gravity) >= 0) { grounded = true; } //check if velocity is in the same direction as gravity (falling)

                velocity = Vector2.zero; //stop moving

                //collide with the closest
                if (hits[i].distance < distance)
                {
                    distance = hits[i].distance; //set new closest distance
                    newGroundNormal = objectNormal; //set new ground normal

                    if (inheritGravity)
                    {
                        gravity = -objectNormal * gravity.magnitude; //inherit gavity of new surface
                        rb2D.rotation += Vector2.SignedAngle(-transform.up, gravity); //match rotation
                    }
                }

                if (hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Spikes")) { HitSpikes(); } //collision with spikes
            }  
        }
        rb2D.position += moveVector.normalized * (distance - buffer); //move object by the shortest distance
        groundNormal = newGroundNormal; //set the ground normal to normal of closest surface
        #endregion

        #region movement collision
        distance = moveVelocity.magnitude; //temporary distance to surface
        groundTangent = grounded ? Tangent(groundNormal) : Tangent(-gravity); //set the ground Tangent
        moveVector = Proj(moveVelocity, groundTangent); //Project the moveVelocity onto the ground

        numCollisions = rb2D.Cast(moveVector, filter, hits, distance);
        for (int i = 0; i < numCollisions; i++)
        {
            //check not collision inside an object
            if (hits[i].distance != 0)
            {
                //collide with the closest
                if (hits[i].distance <= distance)
                {
                    distance = hits[i].distance; //set new closest distance
                }

                //push any moveable objects
                if (hits[i].transform.gameObject.layer == LayerMask.NameToLayer("SolidMoveableObject"))
                {
                    MoveableObject moveObj = hits[i].transform.GetComponent<MoveableObject>();
                    if (moveObj != null) { moveObj.InputVelocity(moveVector); }
                }

                if (hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Spikes")) { HitSpikes(); } //collision with spikes
            }
        }

        if (distance > buffer) { rb2D.position += moveVector.normalized * (distance - buffer); } //move object by the shortest distance
        if (moveVector.magnitude != 0) { sprite.flipX = Vector2.Dot(transform.right, moveVector) < 0; } //face the correct direction
        #endregion  
    }

    #region helper methods

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
