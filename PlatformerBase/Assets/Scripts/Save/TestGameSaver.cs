using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameSaver : MonoBehaviour
{
    void Awake()
    {
        GameSaveData saveData = new GameSaveData
        {
            currentLevel = 1
        };

        GameSaver.SaveGameData(saveData);


        if(GameSaver.LoadGameData().currentLevel == 1)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
