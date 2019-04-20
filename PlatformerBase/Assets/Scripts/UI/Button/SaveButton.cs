using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveButton : GenericButton
{
    private IEnumerable<ILevelData> levelDataObjects; //ref to all the level data objects in the scene
    private BreakableTilemap breakable; //ref to the breakable tilemap

    protected override void Awake()
    {
        levelDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<ILevelData>();
        breakable = FindObjectOfType<BreakableTilemap>();
        base.Awake();
    }

    protected override void OnClick()
    {
        //default save for the game data
        GameSaver.SaveGameData();

        //create a new level save object
        LevelSaveData levelData = new LevelSaveData(GameSaver.currentLevelName);

        //add the states data of all the level data objects 
        foreach (ILevelData data in levelDataObjects)
        {
            levelData.AddObject(data.Name, data.State);
        }

        //add the breakable tilemap data
        if (breakable)
        {
            foreach(Vector2Int broken in breakable.Broken)
            {
                levelData.AddBroken(broken);
            }
        }

        //save the level data
        GameSaver.SaveLevelData(levelData);
    }
}
