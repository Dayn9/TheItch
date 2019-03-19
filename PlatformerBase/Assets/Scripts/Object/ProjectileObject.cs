using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ParticleSystem))]
public class ProjectileObject : MovingObject 
{
    private ProjectileDirection direction; //direcction of the projectile
    [SerializeField] private float acceleration; //rate at which the speed increases 
    [SerializeField] private float maxSpeed; //maximum speed the object should move at 
    private float speed; //how fast the projectile is moving


    private ParticleSystem.ForceOverLifetimeModule forceOverLifetime; //force over lifetime module of particle system

    public ProjectileDirection Direction {
        set {
            direction = value;
            moveVelocity = GetDirectionVector();
            speed = maxSpeed/2;

            forceOverLifetime.x = -moveVelocity.x * maxSpeed;
            forceOverLifetime.y = -moveVelocity.y * maxSpeed;
        }
    }

    private void Awake()
    {
        forceOverLifetime = GetComponent<ParticleSystem>().forceOverLifetime;
    }

    void Update()
    {
        if (!paused)
        {
            //accelerate to max speed
            speed = Mathf.Clamp(speed + (acceleration * Time.deltaTime), 0, maxSpeed);

            //move the pojectile
            transform.position += (Vector3)moveVelocity * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case 17: //collision with breakable object
                collision.GetComponent<BreakableTilemap>().BreakTile(transform.position);
                break;
            case 9: //collision with solid
                gameObject.SetActive(false);
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
