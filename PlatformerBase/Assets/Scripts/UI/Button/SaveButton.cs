using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveButton : GenericButton
{
    private IEnumerable<ILevelData> levelDataObjects;

    protected override void Awake()
    {
        levelDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<ILevelData>();

        base.Awake();
    }

    protected override void OnClick()
    {
        //default save for the game data
        GameSaver.SaveGameData();


        LevelSaveData levelData = new LevelSaveData(GameSaver.currentLevelName);


        //add the states data of all the level data objects 
        foreach (ILevelData data in levelDataObjects)
        {
            levelData.AddObject(data.Name, data.State);
        }

        Debug.Log(levelData.objectNames.Count);

        GameSaver.SaveLevelData(levelData);
    }
}
