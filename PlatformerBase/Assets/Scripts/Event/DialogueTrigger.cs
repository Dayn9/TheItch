using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : IndicatorTrigger {

    [SerializeField] private DialogueBox dialogueBox;

    [Tooltip("img should be 32x32 pixels")]
    [SerializeField] private Sprite faceImage; //image to display in dialogue box

    [SerializeField] [TextArea] private string QuestDialogue; //text dialogue to give quest
    [SerializeField] [TextArea] private string CompletedDialogue; //text dialogue when quest complete2

    // Update is called once per frame
    protected override void Update()
    {
        //check if in contact with the player and player is interacting 
        if (playerTouching)
        {
            if(Input.GetKeyDown(triggers[0]) || Input.GetKeyDown(triggers[1]) || Input.GetMouseButtonDown(0))
            {
                dialogueBox.PauseGame(true);
                CheckQuest();
                if (questCompleted)
                {
                    if (dialogueBox.FirstChunk) { CallAfter(); }
                    dialogueBox.OnTriggerKeyPressed(CompletedDialogue, faceImage);
                }
                else
                {
                    if (dialogueBox.FirstChunk) { CallBefore(); } //only trigger event during the first chunk
                    dialogueBox.OnTriggerKeyPressed(QuestDialogue, faceImage);
                }
            }
            //exit the dialogue if the player jumps out
            else if (Input.GetButtonDown("Jump"))
            {
                dialogueBox.ExitReset();
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //exit dialogue when player leaves
        {
            indicator.SetActive(false);
            playerTouching = false;

            //dialogueBox.Reset(); //reset the dialogue when the player leaves
        }
    }
}
