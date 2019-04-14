using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadButton : GenericButton
{
    protected override void OnClick()
    {
        GameSaveData loadData = GameSaver.LoadGameData(GetComponentInParent<SaveDisplay>().saveNumber);
        GameSaver.currentLevelName = loadData.currentLevel;
        
        startPosition = loadData.PlayerPosition();

        Transition.loadGame = true; //tell transition to load in the game and level data
        InventoryDisplay.loadGame = true;
        Inventory.ClearItemStates();
        BackgroundAudioPlayer.menu = false;
        SceneManager.LoadScene(GameSaver.currentLevelName, LoadSceneMode.Single);
    }
}
