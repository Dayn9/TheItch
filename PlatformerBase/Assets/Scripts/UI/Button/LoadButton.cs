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
        SceneManager.LoadScene(GameSaver.LevelNumToName(loadData.currentLevel), LoadSceneMode.Single);


        Debug.Log("LOAD");
    }
}
