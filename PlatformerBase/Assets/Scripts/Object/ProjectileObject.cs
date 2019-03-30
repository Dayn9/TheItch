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

    private ParticleSystem burstSystem;
    private SpriteRenderer render;

    private PhysicsObject pushing;

    public ProjectileDirection Direction {
        set {
            direction = value;
            moveVelocity = GetDirectionVector();
            speed = maxSpeed/2;

            myForceOverLifetime.x = -moveVelocity.x * maxSpeed;
            myForceOverLifetime.y = -moveVelocity.y * maxSpeed;

            render.enabled = true;
            myEmission.enabled = true;
            burstSystem.Play();
        }
    }

    private void Awake()
    {
        myForceOverLifetime = GetComponent<ParticleSystem>().forceOverLifetime;
        myEmission = GetComponent<ParticleSystem>().emission;

        burstSystem = GetComponentInChildren<ParticleSystem>();
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
        if(!render.enabled) { return; }
        burstSystem.Play();
        switch (collision.gameObject.layer)
        {
            case 17: //collision with breakable object
                collision.GetComponent<BreakableTilemap>().BreakTile(transform.position);
                break;
            case 9: //collision with solid
                myEmission.enabled = false;
                render.enabled = false;
                //moveVelocity = Vector2.zero;
                //gameObject.SetActive(false);
                break;
            //collision with player
            case 8:
                pushing = collision.GetComponent<PhysicsObject>();
                //phys.Frozen = true;
                collision.GetComponent<IHealthObject>().Damage(1);
                pushing.InputVelocity = moveVelocity * 5;
                pushing.MoveVelocity = Vector2.zero;
                pushing.Frozen = true;
                myEmission.enabled = false;
                render.enabled = false;
                //moveVelocity = Vector2.zero;
                break;
        }
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
