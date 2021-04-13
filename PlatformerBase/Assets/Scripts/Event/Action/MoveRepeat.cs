using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRepeat : MoveEvent
{
    [SerializeField] private bool startImmediatly = false;
    [SerializeField] private float waitTime;
    private float waitTimer = 0;

    private bool towards;

    protected override void Start()
    {
        transform.localPosition = origin; //start at the origin
        towards = true;
        SetCols(colliderOn == ColliderOn.Always);

        move = evTrig.State;

        if (!beforeAfter)
        {
            evTrig.Before += ToggleMove;
        }
        evTrig.After += ToggleMove;

    }

    private void ToggleMove()
    {
        move = !move;
        waitTimer = startImmediatly ? waitTime : 0;
    }

    protected override void FixedUpdate()
    {
        if (!Global.paused)
        {
            //move from current position towards final position
            if (move && waitTimer >= waitTime)
            {
                moveVector = (towards ? final : origin) - (Vector2)transform.localPosition; //get Vector towards final destination
                moveObj.MoveVelocity = Vector2.Lerp(moveObj.MoveVelocity, moveVector.normalized * speed * Time.deltaTime, 0.1f);

                //snap into position when close enough
                if (moveVector.magnitude < speed * Time.deltaTime)
                {
                    transform.localPosition = (towards ? final : origin);
                    towards = !towards;
                    waitTimer = 0;

                    //play the snap sound and stop looping the moving SFX
                    //audioPlayer.Loop = false;
                    //audioPlayer.PlaySound(1);
                }
                else
                {
                    transform.position += (Vector3)moveObj.MoveVelocity.normalized * (moveObj.MoveVelocity.magnitude - Global.BUFFER); //move at speed along mov
                }
            }
            else
            {
                moveObj.MoveVelocity = Vector3.zero;
                waitTimer += Time.deltaTime;
            }
        }
    }

}
