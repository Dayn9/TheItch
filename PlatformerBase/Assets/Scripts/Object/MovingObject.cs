using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : Inventory {

    protected Vector2 moveVelocity = Vector2.zero; //initial moving Velocity

    public Vector2 MoveVelocity { get { return moveVelocity; } set { moveVelocity = value; } }//access to the magnitude of the moveVelocity
}
