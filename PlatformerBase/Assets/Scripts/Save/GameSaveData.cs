using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    /// <summary>
    /// Contains all the data for a save file
    /// </summary>

    public int currentLevel = 0;
    private float[] playerPosition;
    public int health = 0;
    public float bpm = 0;

    public GameSaveData(int level, Vector2 playerPos, int health, float bpm)
    {
        currentLevel = level;
        playerPosition = new float[] { playerPos.x, playerPos.y };
        this.health = health;
        this.bpm = bpm;
    }

    public Vector2 PlayerPosition()
    {
        return new Vector2(playerPosition[0], playerPosition[1]);
    }
}
