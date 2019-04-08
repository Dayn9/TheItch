using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : EventTrigger{

    private SpriteRenderer render;

    private const float fadeAlpha = (172.0f / 255);

    private bool fadeIn = false;
    private bool fadeOut = false;

    [SerializeField] private float fadeRate = 1.0f;

    private Jump player;

    [SerializeField] protected DialogueBox dialogueBox;
    [SerializeField] protected string areaName; //holds the name of area to display

    public static bool loadGame = false;

    protected override void Awake()
    {
        base.Awake();
        render = GetComponent<SpriteRenderer>();
        render.enabled = true;

        //add self to dialogue box
        dialogueBox.GetComponent<TextboxEvent>().addEvTrig(this);
    }

    private void Start()
    {
        //Find and hook up all the level changes
        LevelChange[] changes = FindObjectsOfType<LevelChange>();
        foreach (EventTrigger evTrig in changes)
        {
            evTrig.Before += new triggered(FadeOut);
        }

        player = Player.GetComponent<Jump>();

        FadeIn(); //begin the fade in when the scene first loads 
    }

    private void LoadGame()
    {

        GameSaveData loadedData = GameSaver.loadedData;

        Heartbeat.BPM = loadedData.bpm;
        player.Health = loadedData.health;
        player.ReturnPosition = loadedData.PlayerReturnPosition();

        loadGame = false;
    }

    private void FadeIn()
    {
        render.enabled = true;
        render.color = new Color(0, 0, 0, 1);

        fadeIn = true;

        player.Frozen = true; //stop the player from moving
        player.InFallZone = true;

        CallBefore();
        dialogueBox.OnTriggerKeyPressed(areaName);


        GameSaver.CurrentLevelName = areaName; //update the current level
        if (loadGame) { LoadGame(); }
              
    }


    private void FadeOut()
    {
        render.enabled = true;
        render.color = new Color(0, 0, 0, 0);

        fadeOut = true;

        player.Frozen = true; //stop the player from moving
        player.InFallZone = true;
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
                    
                    player.Frozen = false; //stop the player from moving
                    player.InFallZone = false;
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
                    player.Frozen = false; //stop the player from moving
                    player.InFallZone = false;
                    return;
                }
                render.color = new Color(0, 0, 0, render.color.a + change);
            }
        }
    }
}
