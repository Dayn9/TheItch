using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : GenericButton
{
    protected override void OnClick()
    {
        GameSaveData saveData = new GameSaveData(1, Player.transform.position);
        GameSaver.SaveGameData(saveData);

        Debug.Log("SAVE");
    }
}
