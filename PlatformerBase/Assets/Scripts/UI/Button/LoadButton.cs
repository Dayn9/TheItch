using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadButton : GenericButton
{ 
    protected override void OnClick()
    {
        GameSaveData loadData = GameSaver.LoadGameData();

        startPosition = loadData.PlayerPosition();
        Heartbeat.BPM = loadData.bpm;
        Transition.playerHealth = loadData.health;

        GameSaver.CurrentLevelName = GameSaver.LevelNumToName(loadData.currentLevel);
        SceneManager.LoadScene(GameSaver.CurrentLevelName, LoadSceneMode.Single);
    }
}
