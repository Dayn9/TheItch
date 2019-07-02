using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    /// <summary>
    /// Contains all the data for a save file
    /// </summary>

    public string currentLevel = "Fall";

    //Player Information
    private float[] playerPosition;
    private float[] playerReturnPosition; 
    public float health = 0;
    public float bpm = 0;
    public bool[] unlockedAbilities;
    public int[] itemStates;
    public int gemLock;

    //Inventory Info
    public List<string> itemNames; 

    public GameSaveData(string currentLevel, Vector2 playerPosition, Vector2 playerReturnPosition, 
        float health, float bpm, bool[] unlockedAbilities, int[] itemStates, int gemLock)
    {
        this.currentLevel = currentLevel;
        this.playerPosition = new float[] { playerPosition.x, playerPosition.y };
        this.playerReturnPosition = new float[] { playerReturnPosition.x, playerReturnPosition.y };
        this.health = health;
        this.bpm = bpm;
        this.unlockedAbilities = unlockedAbilities;
        this.itemStates = itemStates;
        this.gemLock = gemLock;
    }

    public Vector2 PlayerReturnPosition()
    {
        return new Vector2(playerReturnPosition[0], playerReturnPosition[1]);
    }

    public Vector2 PlayerPosition()
    {
        return new Vector2(playerPosition[0], playerPosition[1]);
    }
}
