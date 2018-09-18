using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEvent : MonoBehaviour {
    [SerializeField] private EventTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    [SerializeField] private Vector3 origin; //position moving from
    [SerializeField] private Vector3 final; //position moving to
    [SerializeField] private float speed; //how fast the object moves to final
    Vector3 moveVector = Vector3.zero; //moveVector to final

    private bool move = false;

    private MovingObject moveObj; //ref to the objects moveVelocity of parent

    private void Awake()
    {
        moveObj = GetComponentInChildren<MovingObject>();
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
        
        transform.position = origin; //start at the origin
    }
	
    //called by event, starts the movement
    private void Move()
    {
        move = true;
    }

    private void Update()
    {
        //move from current position towards final position
        if (move) 
        {
            moveVector = final - transform.position; //get Vector towards final destination
            //snap into position when close enough
            if(moveVector.magnitude < speed * Time.deltaTime)
            {
                transform.position = final;
                move = false;
                moveObj.MoveVelocity = Vector3.zero;
            }
            //move at speed along moveVector
            else
            {
                transform.position += (moveVector.normalized * speed * Time.deltaTime);
            }
            
        }
    }
}
