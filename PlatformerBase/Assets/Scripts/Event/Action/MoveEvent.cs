using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(AudioPlayer))]
[RequireComponent(typeof(Rigidbody2D))]
public class MoveEvent : Global {

    protected enum ColliderOn { Snap, Initial, Always }

    [SerializeField] protected EventTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField] protected bool beforeAfter; //when (before/after questCompleted) the event is triggered

    [SerializeField] protected Vector2 origin; //position moving from
    [SerializeField] protected Vector2 final; //position moving to
    [SerializeField] [Range(0, 5)]protected int speed; //how fast the object moves to final
    protected Vector3 moveVector = Vector3.zero; //moveVector to final

    [Header("Options")]
    [SerializeField] protected ColliderOn colliderOn = ColliderOn.Snap;
    [SerializeField] protected bool fadeTilemap;
    [SerializeField] protected Color initialColor = Color.white;
    [SerializeField] protected Color finalColor = Color.white;
    [SerializeField] protected Color snapColor = Color.white;

    protected bool move = false;

    protected MovingObject moveObj; //ref to the objects moveVelocity of parent
    protected Tilemap rend; //ref to renderer in the object
    protected Collider2D[] colls;

    protected AudioPlayer audioPlayer;

    protected void Awake()
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


        //get the Collider from somewhere in the object if Needed
        colls = GetComponentsInChildren<Collider2D>();
        Assert.IsNotNull(colls);

        audioPlayer = GetComponent<AudioPlayer>();
    }

     protected virtual void Start () {

        if (evTrig.State)
        {
            transform.localPosition = final;
            if (fadeTilemap) { rend.color = snapColor; } //set the color to the snapped color
            SetCols(true);
        }
        else
        {
            transform.localPosition = origin; //start at the origin
            if (fadeTilemap) { rend.color = initialColor; }
            SetCols(colliderOn == ColliderOn.Always);
        }


        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += Move;
        }
        else
        {
            evTrig.After += Move;
        }
     }
	
    //called by event, starts the movement
    protected virtual void Move()
    {
        move = true;

        if (colliderOn == ColliderOn.Initial) { SetCols(true); }

        //loop the moving sound
        audioPlayer.PlaySound(0);
        audioPlayer.Loop = true;
    }

    protected virtual void FixedUpdate()
    {
        if (!paused)
        {
            //move from current position towards final position
            if (move)
            {
                moveVector = final - (Vector2)transform.localPosition; //get Vector towards final destination
                moveObj.MoveVelocity = Vector2.Lerp(moveObj.MoveVelocity, moveVector.normalized * speed * Time.deltaTime, 0.1f);

                //snap into position when close enough
                if (moveVector.magnitude < speed * Time.deltaTime)
                {
                    transform.localPosition = final;
                    move = false;
                   

                    if (fadeTilemap) { rend.color = snapColor; } //set the color to the snapped color
                    if (colliderOn == ColliderOn.Snap) { SetCols(true); }

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
                    transform.position += (Vector3)moveObj.MoveVelocity.normalized * (moveObj.MoveVelocity.magnitude - buffer); //move at speed along mov
                }

            }
            else
            {
                moveObj.MoveVelocity = Vector3.zero;
            }
        }
    }


    protected void SetCols(bool active)
    {
        foreach (Collider2D coll in colls)
        {
            if (coll.GetType() != (new CompositeCollider2D()).GetType())
            {
                coll.enabled = active;
            }
        }
    }


}
