using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharachter : PhysicsObject {

    #region private variables
    [SerializeField] private GameObject indicatorPrefab; //object that appears when the player can interact with object
    [SerializeField] private Vector3 indicatorOffset; //offset from door to display above object
    private GameObject indicator; //ref to instatiated indicatorPrefab

    [SerializeField] private DialogueBox dialogueBox;

    [Tooltip("img should be 32x32 pixels")]
    [SerializeField] private Sprite faceImage; //image to display in dialogue box

    [SerializeField][TextArea] private string QuestDialogue; //text dialogue to give quest
    private bool questCompleted = false; //true when quest has been completed
    [SerializeField] private List<GameObject> itemsRequired; //TODO: redo with (gamobjects?)
    [SerializeField][TextArea] private string CompletedDialogue; //text dialogue when quest complete

    private const KeyCode dialogueTrigger = KeyCode.DownArrow; //Key that will start and advance the dialogue
    private bool playerTouching = false; //true when dialogue is touching Charachter


    #endregion

    protected override void Start()
    {
        base.Start(); //PhysicsObject Start()

        //create the indicator and position it propperly 
        indicator = Instantiate(indicatorPrefab, transform);
        indicator.transform.position = transform.position + indicatorOffset;
        indicator.SetActive(false);
        indicator.name = "Dialogue Indicator";
    }

    protected override void Update()
    {
        if (!paused)
        {
            //check if in contact with the player and player is interacting 
            if (playerTouching && Input.GetKeyDown(dialogueTrigger))
            {
                dialogueBox.gameObject.SetActive(true);
                CheckQuest();
                dialogueBox.OnTriggerKeyPressed(questCompleted ? CompletedDialogue : QuestDialogue, faceImage);
            }
            base.Update();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            indicator.SetActive(true);
            playerTouching = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //exit dialogue when player leaves
        {
            indicator.SetActive(false);
            playerTouching = false;

            dialogueBox.Reset(); //reset the dialogue when the player leaves
        }
    }

    /// <summary>
    /// test if conditions are met for quest to be completed
    /// </summary>
    private void CheckQuest()
    {
        if (!questCompleted) //only check for completion when incomplete
        {
            questCompleted = CheckItems();
        }
    }

    /// <summary>
    /// check if all the required items are in the inventory of player
    /// </summary>
    /// <returns></returns>
    private bool CheckItems()
    {
        //check if all the required items are in the players inventory
        foreach (GameObject item in itemsRequired)
        {
            if (!Items.ContainsKey(item.name))
            {
                return false; //a required item is missing from player inventory
            }
        }
        foreach(GameObject item in itemsRequired)
        {
            RemoveItem(item.name);
        }

        return true; //all required items are in the players inventory
    }
}