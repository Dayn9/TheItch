using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadButton : GenericButton
{ 
    protected override void OnClick()
    {
        GameSaveData loadData = GameSaver.LoadGameData();

        Transition.loadGame = true; //tell transition to load in the game and level data

        startPosition = loadData.PlayerPosition();
        GameSaver.CurrentLevelName = GameSaver.LevelNumToName(loadData.currentLevel);
        SceneManager.LoadScene(GameSaver.CurrentLevelName, LoadSceneMode.Single);
    }
}
