using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public string levelName;

    public List<string> objectNames;
    public List<bool> objectStates;

    public LevelSaveData(string name)
    {
        levelName = name;

        objectNames = new List<string>();
        objectStates = new List<bool>();
    }

    public void AddObject(string name, bool state)
    {
        objectNames.Add(name);
        objectStates.Add(state);
    }

}
