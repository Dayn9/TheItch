using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public string levelName;

    public List<string> objectNames;
    public List<bool> objectStates;
    public List<int[]> broken;

    public LevelSaveData(string name)
    {
        levelName = name;

        objectNames = new List<string>();
        objectStates = new List<bool>();
        broken = new List<int[]>();
    }

    public void AddObject(string name, bool state)
    {
        objectNames.Add(name);
        objectStates.Add(state);
    }

    public void AddBroken(Vector2Int pos)
    {
        broken.Add(new int[] { pos.x, pos.y} );
    }

}
