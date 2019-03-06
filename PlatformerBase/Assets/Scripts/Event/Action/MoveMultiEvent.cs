﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMultiEvent : MoveEvent
{
    [SerializeField] private EventTrigger[] evTrigs;

    private float numTriggered = 0;

    protected override void Start()
    {
        foreach (EventTrigger evTrig in evTrigs)
        {
            if (beforeAfter)
            {
                evTrig.Before += new triggered(Move);
            }
            else
            {
                evTrig.After += new triggered(Move);
            }
        }

        Setup();
    }



    //called by event, starts the movement
    protected override void Move()
    {
        move = true;

        //loop the moving sound
        audioPlayer.PlaySound(0);
        audioPlayer.Loop = true;

        numTriggered += 1;
    }

    protected override void Update()
    {
        if (!paused)
        {
            //move from current position towards final position
            if (move)
            {
                moveVector = Vector2.Lerp(origin, final, numTriggered / evTrigs.Length) 
                    - (Vector2)transform.localPosition; //get Vector towards final destination

                //snap into position when close enough
                if (moveVector.magnitude < speed * Time.deltaTime)
                {
                    transform.localPosition = Vector2.Lerp(origin, final, numTriggered / evTrigs.Length);
                    move = false;
                    moveObj.MoveVelocity = Vector3.zero;

                    if(numTriggered == evTrigs.Length)
                    {
                        if (fadeTilemap) { rend.color = snapColor; } //set the color to the snapped color
                        if (snapCollider)
                        {
                            foreach (Collider2D coll in colls)
                            {
                                coll.enabled = true;
                            }
                        }
                    }

                    //play the snap sound and stop looping the moving SFX
                    audioPlayer.Loop = false;
                    audioPlayer.PlaySound(1);
                }
                else
                {
                    if (fadeTilemap)
                    {
                        float percent = ((Vector2)transform.localPosition - origin).magnitude / (final - origin).magnitude;
                        rend.color = ((1 - percent) * initialColor) + (percent * finalColor);
                    }
                    transform.localPosition += (moveVector.normalized * speed * Time.deltaTime);//move at speed along moveVector
                }

            }
        }
    }
}