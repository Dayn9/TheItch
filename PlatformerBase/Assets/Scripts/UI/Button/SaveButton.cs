using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveButton : GenericButton
{
    protected override void OnClick()
    {
        GameSaver.SaveGameData();

        LevelSaveData levelData = new LevelSaveData(GameSaver.CurrentLevelName);

        foreach(ILevelData data in FindObjectsOfType<MonoBehaviour>().OfType<ILevelData>())
        {
            Debug.Log(data.ToString());
            levelData.AddObject(data.ToString(), data.State);
        }

        GameSaver.SaveLevelData(levelData);
    }
}
