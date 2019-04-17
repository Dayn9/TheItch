using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDialogueTrigger : ZoneTrigger, IDialogue {

    [SerializeField] protected DialogueBox dialogueBox;

    [Tooltip("img should be 32x32 pixels")]
    [SerializeField] protected Sprite faceImage; //image to display in dialogue box

    [SerializeField] [TextArea] protected string enterDialogue; //text dialogue to give quest
    [SerializeField] [TextArea] protected string completedDialogue; 

    private PhysicsObject myPhysObj;
    private static PhysicsObject playerPhysObj;

    //IDialogue Variables
    public DialogueBox DialogueBox { set { dialogueBox = value; } }
    public Sprite FaceImage { get { return faceImage; } }
    public string QuestDialogue { get { return enterDialogue; } }
    public string CompletedDialogue { get { return completedDialogue; } }

    protected override void Awake()
    {
        base.Awake();

        //find the physics object somewhere in the hierarchy of object
        myPhysObj = GetComponent<PhysicsObject>();
        if (myPhysObj == null) { myPhysObj = GetComponentInParent<PhysicsObject>(); }
        if (myPhysObj == null) { myPhysObj = GetComponentInChildren<PhysicsObject>(); }

        if (!dialogueBox)
        {
            dialogueBox = MainCamera.GetComponentInChildren<DialogueBox>();
            Debug.Log(gameObject.name + "'s dialogue trigger needs manual assignment");
        }

        dialogueBox.GetComponent<TextboxEvent>().AddEvTrig(this);
    }

    protected virtual void Start()
    {
        if (playerPhysObj == null)
        {
            playerPhysObj = Player.GetComponent<PhysicsObject>();
        }
    }

    protected override void Update()
    {
        if(!paused && playerTouching && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetMouseButtonDown(0)))
        {
            //check for quest completion and display appropriate dialogue
            CheckQuest();
            dialogueBox.OnTriggerKeyPressed(questCompleted ? completedDialogue : enterDialogue, faceImage);
        }
    }

    //call the before event when player enters zone
    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            CallBefore();
            playerTouching = true;
            dialogueBox.Reset(); //make sure the dialogue box is wipeed

            //check for quest completion and display appropriate dialogue
            CheckQuest();
            dialogueBox.OnTriggerKeyPressed(questCompleted ? completedDialogue : enterDialogue, faceImage);
        }
    }

    //call the after event when player exits zone
    protected override void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            playerTouching = false;
            if (disableAfter) { gameObject.SetActive(false); }
        }
    }

    /// <summary>
    /// sets both the player and this objects physics object frozen property
    /// </summary>
    /// <param name="frozen"></param>
    public void SetFrozen(bool frozen)
    {
        playerPhysObj.Frozen = frozen;
        myPhysObj.Frozen = frozen;
    }
}
