using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : GenericButton
{
    protected override void OnClick()
    {
        Jump player = Player.GetComponent<Jump>();

        GameSaveData saveData = new GameSaveData(
            GameSaver.LevelNameToNum(GameSaver.CurrentLevelName),
            player.ReturnPosition,
            player.Health,
            Heartbeat.BPM
        );

        GameSaver.SaveGameData(saveData);
    }
}
