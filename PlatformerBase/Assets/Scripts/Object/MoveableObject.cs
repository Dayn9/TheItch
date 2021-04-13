using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : PhysicsObject
{
    [SerializeField] [Range(0.0f, 1.0f)] private float drag; //how quickly the object slows down
    

    protected override void FixedUpdate()
    {
        if (!Global.paused)
        {
            base.FixedUpdate(); //Do Physics stuff

            
            //TO DO why?????
            inputVelocity.x = Mathf.Lerp(inputVelocity.x, 0, drag);
            inputVelocity.x = Mathf.Abs(inputVelocity.x) < 0.01 ? 0.0f : inputVelocity.x;
            inputVelocity.y = Mathf.Lerp(inputVelocity.y, 0, drag);
            inputVelocity.y = Mathf.Abs(inputVelocity.y) < 0.01 ? 0.0f : inputVelocity.y;
        }
    }

   
}
