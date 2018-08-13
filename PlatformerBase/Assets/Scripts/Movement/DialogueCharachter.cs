using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharachter : PhysicsObject {

    [SerializeField] private DialogueBox dialogueBox;

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
                Debug.Log(QuestDialogue);
                dialogueBox.DisplayMessage(QuestDialogue);
                questCompleted = true;
            }
            else
            {
                Debug.Log(CompletedDialogue);
                dialogueBox.DisplayMessage(CompletedDialogue);
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
