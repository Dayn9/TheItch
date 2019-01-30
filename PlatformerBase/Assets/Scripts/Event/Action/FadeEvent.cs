﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeEvent : Global {

    [SerializeField] private TransferTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField]
    private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    [SerializeField] private bool snapCollider; //true when the collider is only active after the object fully fades in to the scene 
    [SerializeField] private Color initialColor = Color.white; //starting color of the object
    [SerializeField] private Color finalColor = Color.white; //target end color for the object
    [SerializeField] private Color snapColor = Color.white; //optional color to snap to instead of final color when reached

    private Tilemap render; //ref to the sprite renderer of the object
    private Collider2D coll; //ref to collider of the object

    private bool fade = false;

    private HeartbeatIndicator hbIndicator;

    // Use this for initialization
    void Awake () {
        render = GetComponent<Tilemap>();
        if (snapCollider)
        {
            coll = GetComponent<Collider2D>();
        }
	}

    void Start()
    {
        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += new triggered(Fade);
        }
        else
        {
            evTrig.After += new triggered(Fade);
        }

        //set the initial values for collider state and color 
        render.color = initialColor;
        if (snapCollider)
        {
            coll.enabled = false;
        }
    }

    /// <summary>
    /// called by event, starts fading the object
    /// </summary>
    void Fade()
    {
        fade = true;
        hbIndicator = evTrig.HbIndicator;
    }

    // Update is called once per frame
    void Update () {
		if(!paused && fade)
        {
            float percent = hbIndicator.CurrentHealth / hbIndicator.Total;
            if(percent >= 1.0f)
            {
                render.color = snapColor;
                if (snapCollider)
                {
                    coll.enabled = true;
                }
                fade = false; //stop fading the object in
            }
            else
            {
                render.color = ((1 - percent) * initialColor) + (percent * finalColor);
            }
            
        }
	}
}