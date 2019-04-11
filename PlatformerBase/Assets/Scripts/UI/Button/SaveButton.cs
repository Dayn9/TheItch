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
        GameSaver.SaveLevelData();
    }
}
