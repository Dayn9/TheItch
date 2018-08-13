using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharachter : PhysicsObject {

    #region private variables
    [SerializeField] private DialogueBox dialogueBox;

    [Tooltip("img should be 32x32 pixels")]
    [SerializeField] private Sprite faceImage; //image to display in dialogue box

    [SerializeField][TextArea] private string QuestDialogue; //text dialogue to give quest
    private bool questCompleted = false; //true when quest has been completed
    [SerializeField] private List<string> itemsRequired; 
    [SerializeField][TextArea] private string CompletedDialogue; //text dialogue when quest complete

    private Dictionary<string, GameObject> items; //ref to player inventory
    #endregion

    private void Awake()
    {
        items = Manager.Instance.Items; //gain access to player inventory
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            dialogueBox.gameObject.SetActive(true);
            CheckQuest();
            dialogueBox.DisplayMessage(questCompleted ? CompletedDialogue : QuestDialogue, faceImage);
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
    //test if conditions are met for quest to be completed
    private void CheckQuest()
    {
        if (!questCompleted) //only check for completion when incomplete
        {
            questCompleted = CheckItems();
        }
    }

    private bool CheckItems()
    {
        //check if all the required items are in the players inventory
        foreach (string item in itemsRequired)
        {
            if (!items.ContainsKey(item))
            {
                return false; //a required item is missing from player inventory
            }
        }
        //TODO: remove all required items from items

        return true; //all required items are in the players inventory
    }
}
