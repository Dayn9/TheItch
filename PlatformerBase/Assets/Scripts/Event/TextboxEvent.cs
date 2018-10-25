using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueBox))]
public class TextboxEvent : Global {

    [SerializeField] private ZoneDialogueTrigger evTrig; //eventTrigger 

    [SerializeField] private Vector3 animatedOffset;
    [SerializeField] private float speed;

    private Vector3 targetPos;
    private Vector2 moveVector; //temp vector to the target 

    private bool moveIn = false;
    private bool moveOut = false;

    //TODO timer

    void Start()
    {
        evTrig.Before += new triggered(MoveIn);
    }

    private void Update()
    {
        if (!paused)
        {
            if (moveIn || moveOut)
            {
                moveVector = targetPos - transform.localPosition;
                //snap into position when close enough
                if (moveVector.magnitude < speed * Time.deltaTime)
                {
                    transform.localPosition = targetPos;

                    

                    if (moveOut)
                    {
                        GetComponent<DialogueBox>().Reset();
                        evTrig.Brake = true; //TODO USE EVENT CALLS
                    }

                    moveIn = false;
                    moveOut = false;
                }
                else
                {
                    transform.localPosition += (Vector3)(moveVector.normalized * speed * Time.deltaTime);//move at speed along moveVector
                }
            }
            //temp brake trigger
            if (Input.GetKeyDown(KeyCode.X))
            {
                MoveOut();
            }
        }
    }

    /// <summary>
    /// called by event, sets acivation state
    /// </summary>
    private void MoveIn()
    {
        targetPos = transform.localPosition;
        transform.localPosition += animatedOffset;
        moveIn = true;
    }

    private void MoveOut()
    {
        targetPos = transform.localPosition + animatedOffset;
        moveOut = true;
    }
}
