using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadButton : GenericButton
{ 
    protected override void OnClick()
    {
        GameSaveData loadData = GameSaver.LoadGameData();
        GameSaver.CurrentLevelName = loadData.currentLevel;
        
        startPosition = loadData.PlayerPosition();

        Transition.loadGame = true; //tell transition to load in the game and level data
        InventoryDisplay.loadGame = true;
        SceneManager.LoadScene(GameSaver.CurrentLevelName, LoadSceneMode.Single);
    }
}
