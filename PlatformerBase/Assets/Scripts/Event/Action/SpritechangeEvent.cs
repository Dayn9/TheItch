using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpritechangeEvent : MonoBehaviour {
    [SerializeField] private EventTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered
    [SerializeField] private Sprite newSprite;

    void Start()
    {
        //check if saved
        if (evTrig.State)
        {
            SetSprite();
        }

        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += SetSprite;
        }
        else
        {
            evTrig.After += SetSprite;
        }
    }

    //called by event, changes the sprite
    private void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }

}
