using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        dialogueBox.GetComponent<TextboxEvent>().AddEvTrig(this);

        player = Player.GetComponent<Jump>();
        GameSaver.currentLevelName = areaName; 
        if (loadGame) {
            LoadGame();
        }

        //load in the level data
        LoadLevel();
    }

    private void Start()
    {
        //Find and hook up all the level changes
        LevelChange[] changes = FindObjectsOfType<LevelChange>();
        foreach (EventTrigger evTrig in changes)
        {
            evTrig.Before += new triggered(FadeOut);
        }

        FadeIn(); //begin the fade in when the scene first loads 
    }

    private void LoadGame()
    {
        GameSaveData gameData = GameSaver.gameData;

        Heartbeat.BPM = gameData.bpm;
        player.Health = gameData.health;
        player.ReturnPosition = gameData.PlayerReturnPosition();

        AbilityHandler.Unlocked = gameData.unlockedAbilities;

        loadGame = false;
    }

    /// <summary>
    /// loads in the level data if it exists
    /// </summary>
    private void LoadLevel()
    {
        LevelSaveData levelData = null;
        if((levelData = GameSaver.LoadLevelData(areaName)) != null)
        {
            //create a dictionary of all the loaded in level dat
            Dictionary<string, bool> stateLookup = new Dictionary<string, bool>();
            for(int i = 0; i<levelData.objectNames.Count; i++)
            {
                stateLookup.Add(levelData.objectNames[i], levelData.objectStates[i]);
            }

            //Debug.Log((FindObjectsOfType<MonoBehaviour>().OfType<ILevelData>()).Count<ILevelData>());

            //loop through all the level data objects
            foreach (ILevelData data in FindObjectsOfType<MonoBehaviour>().OfType<ILevelData>())
            {
                //load objects based on state from the dictionary
                if (stateLookup.ContainsKey(data.Name))
                {
                    data.OnLevelLoad(stateLookup[data.Name]);
                }
            }

            BreakableTilemap breakable = FindObjectOfType<BreakableTilemap>();
            if (breakable)
            {
                List<Vector2Int> tempBroken = new List<Vector2Int>();
                foreach(int[] pos in levelData.broken)
                {
                    tempBroken.Add(new Vector2Int(pos[0], pos[1]));
                }
                breakable.Broken = tempBroken;
            }
        }
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
            if (fadeIn && Time.deltaTime < 0.2f) //TODO maybe this isn't the best thing to check for
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
                    GameSaver.SaveGameData();
                    GameSaver.SaveLevelData(); //save the game
                    CallAfter();
                    player.Frozen = false; 
                    player.InFallZone = false;
                    return;
                }
                render.color = new Color(0, 0, 0, render.color.a + change);
            }
        }
    }
}
