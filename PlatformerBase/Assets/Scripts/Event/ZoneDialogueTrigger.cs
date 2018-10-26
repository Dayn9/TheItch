using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FallZone))]
public class ZoneDialogueTrigger : ZoneTrigger {

    [SerializeField] private DialogueBox dialogueBox;

    [SerializeField] [TextArea] private string enterDialogue; //text dialogue to give quest


    //call the before event when player enters zone
    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            CallBefore();
            
            dialogueBox.OnTriggerKeyPressed(enterDialogue);
        }
    }

    //call the after event when player exits zone
    protected override void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            CallAfter();
        }
    }
}
