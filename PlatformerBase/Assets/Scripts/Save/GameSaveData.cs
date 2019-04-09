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
    public int health = 0;
    public float bpm = 0;

    //Inventory Info
    public List<string> itemNames; 

    public GameSaveData(string level, Vector2 playerPos, Vector2 resetPosition, int health, float bpm)
    {
        currentLevel = level;
        playerPosition = new float[] { playerPos.x, playerPos.y };
        playerReturnPosition = new float[] { resetPosition.x, resetPosition.y };
        this.health = health;
        this.bpm = bpm;
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
