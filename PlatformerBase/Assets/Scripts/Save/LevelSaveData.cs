using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSaveData
{
    public string levelName;

    public List<string> objectNames;
    public List<bool> objectStates;

}
