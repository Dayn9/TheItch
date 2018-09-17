using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEvent : MonoBehaviour {
    [SerializeField] private EventTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    [SerializeField] private Vector2 origin;
    [SerializeField] private Vector2 final;
    [SerializeField] private float speed;

    private bool move = false;

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

        transform.position = origin;
    }
	
    private void Move()
    {
        move = true;
    }
    private void Update()
    {
        if (move) //TODO: move from origin to final position at constant speed
        {
            
        }
    }
}
