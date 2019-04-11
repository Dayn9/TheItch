using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSaveButton : GenericButton
{
    protected override void OnClick()
    {
        SaveDisplay display = GetComponentInParent<SaveDisplay>();

        //remove the directory through the gamesaver
        GameSaver.FolderNumber = display.saveNumber;
        GameSaver.RemoveDirectory();

        display.Refresh(); //refresh the display
    }
}
