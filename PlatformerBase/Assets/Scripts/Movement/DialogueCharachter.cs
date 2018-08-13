using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharachter : PhysicsObject {

    [SerializeField] private DialogueBox dialogueBox;

    [Tooltip("img should be 32x32 pixels")]
    [SerializeField] private Sprite faceImage; //image to display in dialogue box

    [SerializeField][TextArea] protected string QuestDialogue; //text dialogue to give quest
    protected bool questCompleted = false; //true when quest has been completed
    [SerializeField][TextArea] protected string CompletedDialogue; //text dialogue when quest complete

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            dialogueBox.gameObject.SetActive(true); 
            if (!questCompleted)
            {
                dialogueBox.DisplayMessage(QuestDialogue, faceImage);
                questCompleted = true;
            }
            else
            {
                dialogueBox.DisplayMessage(CompletedDialogue, faceImage);
            }
        }
    }

    //TEMP exit
    protected virtual void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //exit dialogue when player leaves
        {
            dialogueBox.gameObject.SetActive(false);
        }
    }
}
