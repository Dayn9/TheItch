using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class ProjectileObject : MovingObject 
{
    private ProjectileDirection direction;
    [SerializeField] private float maxSpeed;

    public ProjectileDirection Direction {
        set {
            direction = value;
            moveVelocity = GetDirectionVector();
        }
    }

    void Update()
    {
        if (!paused)
        {
            //move the pojectile
            transform.position += (Vector3)moveVelocity * maxSpeed * Time.deltaTime;
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
