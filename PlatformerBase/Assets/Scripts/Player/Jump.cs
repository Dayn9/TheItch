using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Reseter))]
[RequireComponent(typeof(PlayerAudioPlayer))]
[RequireComponent(typeof(AbilityHandler))]
public class Jump : PhysicsObject, IHealthObject, IPlayer
{
    #region private fields
    [Header("Movement")]
    [SerializeField] private float moveSpeed; //how fast the object can move

    [SerializeField] private float jumpSpeed; //initial jump speed
    [Range(-gravityMag, gravityMag)] [SerializeField] private float addedSpeed; //gravity added on the way up

    private Vector2 movementInput; //user input that will move the player
    private bool jumping = false; //is the player jumping

    private Animator animator; //reference to attached animator component
    private SpriteRenderer render; //attached sprite renderer

    [Header("Health")]
    [SerializeField] private int maxHealth; //maximum health of the player
    private int health; //health of the object
    [SerializeField] private float invulnerabilityTime; //how long the invulnerability timer lasts
    private float invulnerabilityTimer;
    private bool invulnerable; //true when player is immune to damage

    private bool touchingLadder; //true when player is touching the ladder
    protected bool climbing;  

    [Header("Heartrate ")]
    [SerializeField] private HeartbeatPower heartBeatPower; //ref to the heartbeat power script
    private bool moving = false;
    [SerializeField] private float restoreRate;
    [SerializeField] private float removeRate;
    
    [Header ("Reseting")]
    private Vector2 returnPosition; //position to return to when the player falls off the map
    private bool returning = false; //true when player is returning to returnPosition
    private float returnTime; //Time it takes for player to return to position
    private Vector2 returnVelocity;
    [SerializeField] private int returnVelocityDivider;

    private bool frozen = false;
    private AudioPlayer audioPlayer; //ref to the attached audio player 

    #endregion

    #region Properties 
    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }
    public bool Invulnerable { get { return invulnerable; } set { invulnerable = value; } }
    public HeartbeatPower Power { get { return heartBeatPower; } }
    public bool InFallZone { set { inFallZone = value; } }
    public bool Frozen { set { frozen = value; } }
    public Vector2 ReturnPosition { set { returnPosition = value; } }
    public Animator Animator { get { return animator; } }

    public override Vector2 MoveVelocity { get { return moveVelocity * moveSpeed; } }


    public float MoveSpeed {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    public float JumpSpeed {
        get { return jumpSpeed; }
        set { jumpSpeed = value; }
    }
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
            if (frozen)
            {
                movementInput = inputVelocity;
                jumping = false;
                return;
            }
            //get and then modify cardinal input 
            movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if(climbing && Input.GetButton("Jump")) { movementInput += Vector2.up; }
            movementInput = movementInput.normalized * Mathf.Clamp(movementInput.magnitude, 0, 1.0f) * moveSpeed; //make sure length of input vector is less than 1; 

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
                //climbing = false; //jump out of climbing
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
            if (grounded && Vector2.Dot(gravity, moveVelocity) > 0 && 
                (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
            {
                CollideOneway(false);
            }

            moving = moveVelocity.magnitude > buffer; //determine if object is moving
            //update the health system
            if (moving) { heartBeatPower.RestoreBPM  (restoreRate * Time.deltaTime); }
            else { heartBeatPower.RemoveBPM(removeRate * Time.deltaTime); }

            //start climbing if move velocity is up or down (not just side to side)
            if (touchingLadder && moveVelocity.y != 0)
            {
                climbing = true;
            }
            touchingLadder = false; //reset every time 
            if (climbing)
            {
                //while climbing player can move in all 4 directions
                gravityVelocity = Proj(moveVelocity, groundNormal) - (gravity * Time.deltaTime);
                moveVelocity = Proj(moveVelocity, groundTangent);
            }
            else
            {
                //update moveVelocity to be only in direction of ground normal
                if (grounded) { moveVelocity = Proj(moveVelocity, groundTangent); }        
            }
            base.FixedUpdate();
            if (!touchingLadder)
            {
                climbing = false;
            }
            #endregion

            #region Animation
            //send values to animator
            animator.SetBool("grounded", grounded || climbing);
            if (climbing)
            {
                animator.SetFloat("verticalVel", 0); //cancel fall animation
                //use greatest movement direction for movement animation
                animator.SetFloat("horizontalMove", gravityVelocity.magnitude > moveVelocity.magnitude 
                    ? gravityVelocity.magnitude : moveVelocity.magnitude);
            }
            else
            {
                animator.SetFloat("verticalVel", gravityVelocity.magnitude * (Vector2.Angle(gravity, gravityVelocity) > 90 ? 1 : -1));
                animator.SetFloat("horizontalMove", moveVelocity.magnitude * (Vector2.Dot(transform.right, moveVelocity) < 0 ? 1 : -1));
            }

            //invulnerabilty animation
            render.color = Color.white;
            if (invulnerable)
            {
                invulnerabilityTimer -= Time.deltaTime; //increase timer 
                if (invulnerabilityTimer <= invulnerabilityTime - 0.2f)
                {
                    //damage animation and color change
                    animator.SetBool("damage", false);
                    render.color = Color.gray; //fade Slightly
                }
                //stop the timer and end invulnerability
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

    protected override void TouchLadder()
    {
        touchingLadder = true;
        //start climbing if move velocity is up or down (not just side to side)
        if (moveVelocity.y != 0)
        {
            climbing = true;
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