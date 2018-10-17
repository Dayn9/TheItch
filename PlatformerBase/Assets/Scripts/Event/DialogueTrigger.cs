using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : EventTrigger {

    [SerializeField] private DialogueBox dialogueBox;

    [Tooltip("img should be 32x32 pixels")]
    [SerializeField] private Sprite faceImage; //image to display in dialogue box

    [SerializeField] [TextArea] private string QuestDialogue; //text dialogue to give quest
    [SerializeField] [TextArea] private string CompletedDialogue; //text dialogue when quest complete2

    // Update is called once per frame
    protected override void Update()
    {
        //check if in contact with the player and player is interacting 
        if (playerTouching && Input.GetKeyDown(trigger))
        {
            dialogueBox.PauseGame(true);
            dialogueBox.gameObject.SetActive(true);
            CheckQuest();
            if (questCompleted)
            {
                dialogueBox.OnTriggerKeyPressed(CompletedDialogue, faceImage);
                CallAfter();
            }
            else
            {
                dialogueBox.OnTriggerKeyPressed(QuestDialogue, faceImage);
                CallBefore();
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //exit dialogue when player leaves
        {
            indicator.SetActive(false);
            playerTouching = false;

            dialogueBox.Reset(); //reset the dialogue when the player leaves
        }
    }
}
