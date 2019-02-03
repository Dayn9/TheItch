using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogue
{
    /// <summary>
    /// contains variables related to dialogue objects
    /// </summary>

    DialogueBox DialogueBox { set; }
    Sprite FaceImage { get; }

    string QuestDialogue { get; }
    string CompletedDialogue { get; }

    void SetFrozen(bool frozen);
}
