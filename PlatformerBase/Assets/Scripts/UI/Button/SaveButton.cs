using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveButton : GenericButton
{
    protected override void OnClick()
    {
        //default save for the game data
        GameSaver.SaveGameData();


        //save the data on the current level
        LevelSaveData levelData = new LevelSaveData(GameSaver.CurrentLevelName);

        //add the states data of all the level data objects 
        foreach(ILevelData data in FindObjectsOfType<MonoBehaviour>().OfType<ILevelData>())
        {
            levelData.AddObject(data.Name, data.State);
        }

        GameSaver.SaveLevelData(levelData);
    }
}
