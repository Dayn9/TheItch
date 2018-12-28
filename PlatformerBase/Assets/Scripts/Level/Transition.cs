using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : EventTrigger{

    private SpriteRenderer render;

    [SerializeField] private EventTrigger[] evTrigs; 

    private const float fadeAlpha = (172.0f / 255);

    private bool fadeIn = false;
    private bool fadeOut = false;

    [SerializeField] private float fadeRate = 1.0f;

    private IPlayer playerFreeze;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();

        foreach (EventTrigger evTrig in evTrigs)
        {
            evTrig.Before += new triggered(FadeOut);
        }

        render.enabled = true;
        playerFreeze = Player.GetComponent<IPlayer>();

        FadeIn();
    }

    private void FadeIn()
    {
        render.enabled = true;
        render.color = new Color(0, 0, 0, 1);
        fadeIn = true;
        playerFreeze.Frozen = true; //stop the player from moving
        playerFreeze.InFallZone = true; 
    }


    private void FadeOut()
    {
        render.enabled = true;
        render.color = new Color(0, 0, 0, 0);
        fadeOut = true;
        playerFreeze.Frozen = true; //stop the player from moving
        playerFreeze.InFallZone = true;
    }

    protected override void Update()
    {
        if (!paused)
        {
            if (fadeIn && Time.deltaTime < 0.2f) 
            {
                float change = fadeRate * Time.deltaTime;
                if (render.color.a - change <= 0)
                {
                    render.color = new Color(0, 0, 0, fadeAlpha);
                    fadeIn = false;
                    render.enabled = false;
                    CallBefore();
                    playerFreeze.Frozen = false; //stop the player from moving
                    playerFreeze.InFallZone = false;
                    return;
                }
                render.color = new Color(0, 0, 0, render.color.a - change);
            }
            else if (fadeOut)
            {
                float change = fadeRate * Time.deltaTime;
                if (render.color.a + change >= 1)
                {
                    render.color = new Color(0, 0, 0, 1);
                    fadeOut = false;
                    CallAfter(); 
                    playerFreeze.Frozen = false; //stop the player from moving
                    playerFreeze.InFallZone = false;
                    return;
                }
                render.color = new Color(0, 0, 0, render.color.a + change);
            }
        }
    }
}
