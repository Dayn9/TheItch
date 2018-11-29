using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Reseter))]
[RequireComponent(typeof(PlayerAudioPlayer))]
public class Jump : PhysicsObject, IHealthObject, IPlayer
{
    # region private fields
    [SerializeField] private float moveSpeed; //how fast the object can move

    [SerializeField] private float jumpSpeed; //initial jump speed
    [Range(-gravityMag, gravityMag)] [SerializeField] private float addedSpeed; //gravity added on the way up

    private Vector2 movementInput; //user input that will move the player
    private bool jumping = false; //is the player jumping

    private Animator animator; //reference to attached animator component
    private SpriteRenderer render; //attached sprite renderer

    [SerializeField] private int maxHealth; //maximum health of the player
    private int health; //health of the object
    [SerializeField] private float invulnerabilityTime; //how long the invulnerability timer lasts
    private float invulnerabilityTimer;
    private bool invulnerable; //true when player is immune to damage

    private bool touchingLadder; //true when player is touching the ladder
    protected bool climbing;

    [SerializeField] private bool canSprint; 
    [SerializeField] private float sprintSpeed;

    [SerializeField] private HeartbeatPower heartBeatPower;
    private bool moving = false;
    [SerializeField] private float restoreRate;
    [SerializeField] private float removeRate;

    private Vector2 returnPosition; //position to return to when the player falls off the map
    private bool returning = false; //true when player is returning to returnPosition
    private float returnTime; //Time it takes for player to return to position
    private Vector2 returnVelocity;
    [SerializeField] private int returnVelocityDivider;

    private bool frozen = false;

    private AudioPlayer audioPlayer;

    #endregion

    #region Properties 
    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }
    public bool Invulnerable { get { return invulnerable; } set { invulnerable = value; } }
    public HeartbeatPower Power { get { return heartBeatPower; } }
    public bool InFallZone { set { inFallZone = value; } }
    public bool Frozen { set { frozen = value; } }
    public Vector2 ReturnPosition { set { returnPosition = value; } }
    #endregion

    //Start is already being called in Base PhysicsObject Class
    private void Awake()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();

        if (maxHealth < 1) { maxHealth = 1; } //must have at leath one health point
        health = maxHealth;
        SetReturnPosition(transform.position); //set the return position to 1 unit above where the player initially spawned

        if (startPosition != Vector2.one)
        {
            transform.position = startPosition;
        }

        audioPlayer = GetComponent<AudioPlayer>();
    }

    private void Update()
    {
        if (!paused)
        {
            //don't accept input when frozen
            if (frozen) {
                movementInput = Vector2.zero;
                jumping = false;
                return;
            }

            movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            movementInput = movementInput.normalized * Mathf.Clamp(movementInput.magnitude, 0, 1.0f) //make sure length of input vector is less than 1;
                * (canSprint && Input.GetAxis("Fire3") > 0 ? sprintSpeed : moveSpeed); //multiply be appropritate speed\

            //can input a jump before you hit the ground
            if (Input.GetButtonDown("Jump"))
            {
                jumping = true;
            }
            //releasing jump always cancels jumping
            else if (Input.GetButtonUp("Jump"))
            {
                jumping = false;
            }
        }
    }

    protected override void FixedUpdate()
    {
        if (!paused)
        {
            if (returning)
            {
                //move back to returnPosition
                moveVelocity = Vector2.SmoothDamp(transform.position, returnPosition, ref returnVelocity, returnTime); 
                gravityVelocity = Vector2.up * moveVelocity.y; //set moveVelocities for camera
                transform.position = moveVelocity;
                if(((Vector2)transform.position - returnPosition).magnitude < 1)
                {
                    gravityVelocity = Vector2.zero;
                    returning = false;
                }
                //manual mapipulation of animator to falling animation
                animator.SetBool("grounded", false);
                animator.SetFloat("verticalVel", -1);
                return; //don't more the player conventually 
            }

            //determine moveVeclocity and if the object is moving
            moveVelocity = movementInput * Time.deltaTime;

            if (inFallZone)
            {
                //manual mapipulation of animator to falling animation
                animator.SetBool("grounded", false);
                animator.SetFloat("verticalVel", -1);

                gravityVelocity = Vector2.zero;

                base.FixedUpdate();

                gravityVelocity = gravity.normalized * maxGravity;
                return; //don't let the player move 
            }
            
            #region Movement
            //jumping when on ground
            if (jumping && (grounded || climbing))
            {
                gravityVelocity = (inheritGravity ? groundNormal : Vector2.up) * jumpSpeed *  Time.deltaTime;
                jumping = false; //insures that you can only jump once after pressing jump button
                climbing = false; //jump out of climbing
            }

            //add velocity while moving upwards 
            if (Vector2.Dot(gravity, gravityVelocity) < 0) //check if moving upwards
            {
                CollideOneway(false);
                //add to velocity in direction of velocity proportional to velocity magnitude
                if (Input.GetButton("Jump"))
                {
                    gravityVelocity += addedSpeed * gravityVelocity.normalized * Time.deltaTime;
                } 
            }
            //moving downwards
            else
            {
                CollideOneway(true);
            }

            //fall throught one way platforms when input is down

            if (grounded && Vector2.Dot(gravity, moveVelocity) > 0)
            {
                CollideOneway(false);
            }


            //update moveVelocity to be only in direction of ground normal
            if (grounded) { moveVelocity = Proj(moveVelocity, groundTangent); }
            moving = moveVelocity.magnitude > buffer; //determine if object is moving

            //update the health system
            if (moving) { heartBeatPower.RestoreBPM(restoreRate * Time.deltaTime); }
            else { heartBeatPower.RemoveBPM(removeRate * Time.deltaTime); }

            //start climbing if move velocity is up or down
            if (touchingLadder)
            {
                //if not moving side to side
                if (Vector2.Dot(moveVelocity, gravity) != 0)
                {
                    climbing = true;
                }
            }
            #endregion

            if (climbing && gravityVelocity.magnitude == 0)
            {
                CollideOneway(false); //don't collide with oneways while climbing 
                grounded = false;

                #region climbing collision
                float distance = moveVelocity.magnitude; //temporary distance to surface
                Vector2 moveVector = moveVelocity; //Project the moveVelocity onto the ground

                float numCollisions = rb2D.Cast(moveVector, filter, hits, distance);
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
                    }
                }
                if (distance > buffer) { rb2D.position = moveVector.normalized * (distance - buffer); } //move object by the shortest distance
                if (moveVector.magnitude != 0) { sprite.flipX = Vector2.Dot(transform.right, moveVector) < 0; } //face the correct direction
                #endregion
            }
            else
            {
                base.FixedUpdate();
            }

            #region Animation

            //send values to animator
            animator.SetBool("grounded", grounded);
            animator.SetFloat("verticalVel", gravityVelocity.magnitude * (Vector2.Angle(gravity, gravityVelocity) > 90 ? 1 : -1));
            animator.SetFloat("horizontalMove", moveVelocity.magnitude * (Vector2.Dot(transform.right, moveVelocity) < 0 ? 1 : -1));

            render.color = Color.white;
            if (invulnerable)
            {
                invulnerabilityTimer -= Time.deltaTime;
                if (invulnerabilityTimer <= invulnerabilityTime - 0.2f)
                {
                    animator.SetBool("damage", false);
                    render.color = Color.gray; //fade Slightly
                }

                if (invulnerabilityTimer <= 0)
                {
                    invulnerable = false;
                }
            }
            #endregion
        }
    }

    /// <summary>
    /// choose whether the object should collide with oneway platforms
    /// </summary>
    /// <param name="collide">true if object collides</param>
    private void CollideOneway(bool collide)
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Oneway"), !collide);
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
    }

    protected override void HitSpikes()
    {
        TakeDamage(2);
    }

    //Sent when another object enters a trigger collider attached to this object (2D physics only).
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Ladder")) {

            touchingLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        //stop climbing when no longer touching ladder
        if (coll.gameObject.layer == LayerMask.NameToLayer("Ladder"))
        {
            touchingLadder = false;
            climbing = false;
        }
    }

    #region Health
    public void TakeDamage(int amount)
    {
        if (!invulnerable)
        {
            health -= amount;
            invulnerable = true;
            invulnerabilityTimer = invulnerabilityTime; //start the timer
            animator.SetBool("damage", true);
            //death code
            if (health <= 0)
            {
                health = 0;
                frozen = true;
                animator.SetTrigger("death");
                jumping = false;
                //reset game is called by the death animation
            }
        }
    }

    public void Heal(int amount)
    {
        //add health up to max health
        health = (health + amount) % maxHealth;
    }
    public void Heal(float amount)
    {
        //add health up to max health
        health = (health + (int)amount) % maxHealth;
    }

    public void FullHeal()
    {
        health = maxHealth;
    }
    #endregion

    private void SetReturnPosition(Vector2 set)
    {
        returnPosition = set + Vector2.up;
    }

    public void OnPlayerFall()
    {
        returnVelocity = Vector2.zero; //reset the return Velocity
        returnTime = ((Vector2)transform.position - returnPosition).magnitude / returnVelocityDivider;
        audioPlayer.PlaySound("Return");
        returning = true; //start returning to return position
    }
}
 