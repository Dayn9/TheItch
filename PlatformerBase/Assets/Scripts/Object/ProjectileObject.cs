using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProjectileObject : MovingObject 
{
    void Update()
    {
        if (!paused)
        {
            transform.position += (Vector3)moveVelocity * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 17)
        {
            collision.GetComponent<BreakableTilemap>().BreakTile(transform.position + (Vector3)moveVelocity.normalized);
        }
    }
}
