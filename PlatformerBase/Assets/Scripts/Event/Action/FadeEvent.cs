using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeEvent : Global {

    [SerializeField] private TransferTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField]
    private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    [SerializeField] private Gradient gradient;
    [SerializeField] private bool snapCollider; //true when the collider is only active after the object fully fades in to the scene 
    [SerializeField] private Color snapColor = Color.white; //optional color to snap to instead of final color when reached

    private Tilemap render; //ref to the sprite renderer of the object
    private Collider2D coll; //ref to collider of the object

    private bool fade = false;
    [Header("Fade In/True - FadeOut/False")]
    [SerializeField] private bool fadeInOut; 

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
        hbIndicator = evTrig.HbIndicator;

        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += Fade;
        }
        else
        {
            evTrig.After += Fade;
        }

        //set the initial values for collider state and color 
        if (snapCollider)
        {
            coll.enabled = false;
        }

        if (fadeInOut && evTrig.FullyHealed)
        {
            Fade();
        }
        else if(!fadeInOut)
        {
            render.color = snapColor;
        }
        else
        {
            render.color = gradient.Evaluate(0);
        }
    }

    /// <summary>
    /// called by event, starts fading the object
    /// </summary>
    void Fade()
    {
        fade = true;
    }

    // Update is called once per frame
    void Update () {
		if(!paused && fade)
        {
            float percent = hbIndicator.CurrentHealth / hbIndicator.Total;

            if((fadeInOut && percent >= 1.0f) || (!fadeInOut && percent <= 0.0f))
            {
                render.color = snapColor;
                if (snapCollider)
                {
                    coll.enabled = fadeInOut;
                }
                fade = false; //stop fading the object in
            }
            else
            {
                render.color = gradient.Evaluate(percent);
                //render.color = ((1 - percent) * initialColor) + (percent * finalColor);
            }
            
        }
	}
}
