using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicPlayer : PhysicsObject
{
    [SerializeField] private int speed;

    protected void Awake()
    {

    }

    protected override void Update()
    {
        moveVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * speed;
        transform.position += (Vector3)moveVelocity * Time.deltaTime;
    }

    protected override void FixedUpdate()
    {

    }
}

