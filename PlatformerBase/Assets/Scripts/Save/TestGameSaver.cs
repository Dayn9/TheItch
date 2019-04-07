using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameSaver : Global
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameSaveData saveData = new GameSaveData(1, Player.transform.position);

            GameSaver.SaveGameData(saveData);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameSaveData loadedData = GameSaver.LoadGameData();
            Debug.Log(loadedData.PlayerPosition());
        }
    }
}
