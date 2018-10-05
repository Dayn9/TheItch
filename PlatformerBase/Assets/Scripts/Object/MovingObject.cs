using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : Global {

    protected Vector2 moveVelocity = Vector2.zero; //velocity the object manipulates on it's own

    public Vector2 MoveVelocity { get { return moveVelocity; } set { moveVelocity = value; } }//access to the magnitude of the moveVelocity

}
