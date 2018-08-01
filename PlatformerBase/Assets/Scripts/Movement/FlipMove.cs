using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipMove : PhysicsObject {

    [SerializeField] private float moveSpeed; //how fast the object can move

    private void Update()
    {
        moveVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * moveSpeed * Time.deltaTime;

        if (Input.GetButton("Jump") && grounded)
        {
            gravity = -gravity;
            rb2D.rotation += Vector2.SignedAngle(-transform.up, gravity); //match rotation

            if(moveVelocity.magnitude == 0) { sprite.flipX = !sprite.flipX; } //check if moving to prevent double sprite flip
        }
    }
}
