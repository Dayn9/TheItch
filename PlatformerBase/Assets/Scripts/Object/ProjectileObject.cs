using UnityEngine;

public class ProjectileObject : MovingObject 
{
    void Update()
    {
        if (!paused)
        {
            transform.position += (Vector3)moveVelocity * Time.deltaTime;
        }
    }
}
