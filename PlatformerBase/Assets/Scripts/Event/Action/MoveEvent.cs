﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(AudioPlayer))]
public class MoveEvent : Global {
    [SerializeField] private EventTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    [SerializeField] private Vector2 origin; //position moving from
    [SerializeField] private Vector2 final; //position moving to
    [SerializeField] private float speed; //how fast the object moves to final
    Vector3 moveVector = Vector3.zero; //moveVector to final

    [Header("Options")]
    [SerializeField] private bool snapCollider;
    [SerializeField] private bool fadeTilemap;
    [SerializeField] private Color initialColor = Color.white;
    [SerializeField] private Color finalColor = Color.white;
    [SerializeField] private Color snapColor = Color.white;

    private bool move = false;

    private MovingObject moveObj; //ref to the objects moveVelocity of parent
    private Tilemap rend; //ref to renderer in the object
    private Collider2D[] colls;

    private AudioPlayer audioPlayer;

    private void Awake()
    {
        //get the MoveObject from somewhere in the object
        moveObj = GetComponent<MoveableObject>();
        if (moveObj == null)
        {
            moveObj = GetComponentInChildren<MovingObject>();
        }
        Assert.IsNotNull(moveObj);

        if (fadeTilemap)
        {
            //get the Renderer from somewhere in the object id Needed
            rend = GetComponent<Tilemap>();
            if (rend == null)
            {
                rend = GetComponentInChildren<Tilemap>();
            }
            Assert.IsNotNull(rend);
        }

        if (snapCollider)
        {
            //get the Collider from somewhere in the object if Needed
            colls = GetComponentsInChildren<Collider2D>();
            Assert.IsNotNull(colls);
        }

        audioPlayer = GetComponent<AudioPlayer>();
    }


    void Start () {
        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += new triggered(Move);
        }
        else
        {
            evTrig.After += new triggered(Move);
        }
        
        transform.localPosition = origin; //start at the origin
        if (fadeTilemap) { rend.color = initialColor; }
        if (snapCollider) {
            foreach (Collider2D coll in colls)
            {
                if(coll.GetType() != (new CompositeCollider2D()).GetType())
                {
                    coll.enabled = false;
                }
            }
        }

    }
	
    //called by event, starts the movement
    private void Move()
    {
        move = true;

        //loop the moving sound
        audioPlayer.PlaySound(0);
        audioPlayer.Loop = true;
    }

    private void Update()
    {
        if (!paused)
        {
            //move from current position towards final position
            if (move)
            {
                moveVector = final - (Vector2)transform.localPosition; //get Vector towards final destination

                //snap into position when close enough
                if (moveVector.magnitude < speed * Time.deltaTime)
                {
                    transform.localPosition = final;
                    move = false;
                    moveObj.MoveVelocity = Vector3.zero;

                    if (fadeTilemap) { rend.color = snapColor; } //set the color to the snapped color
                    if (snapCollider)
                    {       
                        foreach (Collider2D coll in colls)
                        {
                            coll.enabled = true;
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

