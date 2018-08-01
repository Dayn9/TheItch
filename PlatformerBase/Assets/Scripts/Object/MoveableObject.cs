using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : PhysicsObject
{
    [SerializeField] [Range(0.0f, 1.0f)] private float drag; //how quickly the object slows down

    protected void Update()
    {
        //slow down
        moveVelocity.x = Mathf.Lerp(moveVelocity.x, 0, drag);
        moveVelocity.x = Mathf.Abs(moveVelocity.x) < 0.01 ? 0.0f : moveVelocity.x;
        moveVelocity.y = Mathf.Lerp(moveVelocity.y, 0, drag);
        moveVelocity.y = Mathf.Abs(moveVelocity.y) < 0.01 ? 0.0f : moveVelocity.y;
    }

    /// <summary>
    /// add force to moveVelocity
    /// </summary>
    /// <param name="inputVelocity">force to add</param>
    public void InputVelocity(Vector2 inputVelocity)
    {
        moveVelocity += inputVelocity;
    }
}
