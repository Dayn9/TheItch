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
        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += new triggered(SetSprite);
        }
        else
        {
            evTrig.After += new triggered(SetSprite);
        }
    }

    private void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = newSprite;
    }

}
