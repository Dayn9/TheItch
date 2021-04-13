using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {

    protected Vector2 moveVelocity = Vector2.zero; //velocity the object manipulates on it's own
    protected bool frozen = false; 

    public bool Frozen { get { return frozen; } set { frozen = value; } }
    public virtual Vector2 MoveVelocity { get { return moveVelocity; } set { moveVelocity = value; } }//access to the magnitude of the moveVelocity

}
