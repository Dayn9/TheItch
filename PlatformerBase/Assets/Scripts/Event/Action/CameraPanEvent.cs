using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanEvent : Pause
{ 
    [SerializeField] private EventTrigger evTrig; //eventTrigger 

    [Header("Before/True  -  After/False")]
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    private Vector2 origin; //position moving from
    [SerializeField] private Vector2 final; //position moving to
    [SerializeField] private bool useTarget;
    [SerializeField] private Transform target;
    [SerializeField] private float speed; //how fast the object moves to final

    [SerializeField] private float holdTime;
    private float holdTimer = 0;

    Vector3 moveVector = Vector3.zero; //moveVector to final
    private bool move = false;
    private bool movingOut = true;
    private BaseCamera camController; //ref to camera controller 

    void Start()
    {
        camController = MainCamera.GetComponent<BaseCamera>();

        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += new triggered(Move);
        }
        else
        {
            evTrig.After += new triggered(Move);
        }
    }

    //called by event, starts the movement
    private void Move()
    {
        move = true;
        movingOut = true;

        origin = camController.transform.position;

        if (useTarget) { final = target.transform.position; }

        camController.Manual = true; //take over control
        PauseGame(true);
    }

    public void Update()
    {
        if (move && holdTimer == 0)
        {
            moveVector = (movingOut ? final : origin) - (Vector2)camController.transform.localPosition; //get Vector towards final destination
            //snap into position when close enough
            if (moveVector.magnitude < speed * Time.deltaTime || Mathf.Abs(moveVector.x) < speed * Time.deltaTime || Mathf.Abs(moveVector.y) < speed * Time.deltaTime)
            {
                Vector3 snapPos = (movingOut ? final : origin);
                snapPos.z = camController.transform.position.z;
                camController.transform.localPosition = snapPos;
                if (!movingOut)
                {
                    move = false;
                    camController.Manual = false;
                    PauseGame(false);
                }
                else
                {
                    movingOut = false;
                    holdTimer += Time.deltaTime; //starts the hold period 
                }
            }
            else
            {
                camController.transform.localPosition += (moveVector.normalized * speed * Time.deltaTime);//move at speed along moveVector
            }
        }
        else if(move && holdTimer < holdTime)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                holdTimer = 0; //reset the timer and start moving back out
            }
        }
    }
}
