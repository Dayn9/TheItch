using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ParticleSystem))]
public class ProjectileObject : MovingObject 
{
    private ProjectileDirection direction; //direcction of the projectile
    [SerializeField] private float acceleration; //rate at which the speed increases 
    [SerializeField] private float maxSpeed; //maximum speed the object should move at 
    private float speed; //how fast the projectile is moving

    private ParticleSystem.ForceOverLifetimeModule myForceOverLifetime; //force over lifetime module of particle system
    private ParticleSystem.EmissionModule myEmission;

    private SpriteRenderer render;

    private ProjectilePool projectilePool;

    private const int pushForce = 5;
    private PhysicsObject pushing;

    private bool active = false;

    public ProjectilePool ProjectilePool { set { projectilePool = value; } }
    public bool Active { get { return active; } }

    public ProjectileDirection Direction {
        set {
            direction = value;
            SetState(true);

            speed = maxSpeed/2;
            myForceOverLifetime.x = -moveVelocity.x * maxSpeed;
            myForceOverLifetime.y = -moveVelocity.y * maxSpeed;
        }
    }

    private void Awake()
    {
        myForceOverLifetime = GetComponent<ParticleSystem>().forceOverLifetime;
        myEmission = GetComponent<ParticleSystem>().emission;

        
        render = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!paused)
        {
            //accelerate to max speed
            speed = Mathf.Clamp(speed + (acceleration * Time.deltaTime), 0, maxSpeed);

            //move the pojectile
            transform.position += (Vector3)moveVelocity * speed * Time.deltaTime;

            if(pushing != null)
            {
                pushing.Frozen = false;
                pushing = null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CollisionChecks(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollisionChecks(collision);
    }

    private void CollisionChecks(Collider2D collision)
    {
        if(!active) { return; }
        switch (collision.gameObject.layer)
        {
            case 17: //collision with breakable object
                collision.GetComponent<BreakableTilemap>().BreakTile(transform.position);
                break;
            case 9: //collision with solid
                SetState(false);
                projectilePool.BurstParticles(transform.position);
                break;
            //collision with player
            case 8:
                //damage the player
                collision.GetComponent<IHealthObject>().Damage(1);
                //push the player
                pushing = collision.GetComponent<PhysicsObject>();
                pushing.InputVelocity = moveVelocity * pushForce;
                pushing.MoveVelocity = Vector2.zero;
                pushing.Frozen = true;
                //deactivate projectile
                SetState(false);
                projectilePool.BurstParticles(transform.position);
                break;
        }
    }

    private void SetState(bool state)
    {
        myEmission.enabled = state;
        render.enabled = state;
        active = state;

        moveVelocity = state ? GetDirectionVector(): Vector2.zero;
    }

    /// <summary>
    /// translates direction into vector
    /// </summary>
    /// <returns>direction vector</returns>
    private Vector2 GetDirectionVector()
    {
        switch (direction)
        {
            case ProjectileDirection.left:
                return Vector2.left;
            case ProjectileDirection.right:
                return Vector2.right;
            case ProjectileDirection.down:
                return Vector2.down;
            case ProjectileDirection.up:
                return Vector2.up;
            default:
                return Vector2.zero;
        }
    }
}
