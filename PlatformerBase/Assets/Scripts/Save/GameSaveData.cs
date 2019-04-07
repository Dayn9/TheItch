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
    private List<int> textList;

    public GameSaveData(int level, Vector2 playerPos)
    {
        currentLevel = level;
        playerPosition = new float[] { playerPos.x, playerPos.y };
    }

    public Vector2 PlayerPosition()
    {
        return new Vector2(playerPosition[0], playerPosition[1]);
    }
}
