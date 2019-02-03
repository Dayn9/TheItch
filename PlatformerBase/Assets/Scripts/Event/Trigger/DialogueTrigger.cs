using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : IndicatorTrigger, IDialogue {
    [SerializeField] private DialogueBox dialogueBox;

    [Tooltip("img should be 32x32 pixels")]
    [SerializeField] private Sprite faceImage; //image to display in dialogue box

    [SerializeField] [TextArea] private string questDialogue; //text dialogue to give quest
    [SerializeField] [TextArea] private string completedDialogue; //text dialogue when quest complete

    private PhysicsObject myPhysObj;
    private static PhysicsObject playerPhysObj;

    //IDialogue variables
    public DialogueBox DialogueBox { set { dialogueBox = value; } }
    public Sprite FaceImage { get { return faceImage; } }
    public string QuestDialogue { get { return questDialogue; } }
    public string CompletedDialogue { get { return completedDialogue;  } }

    protected override void Awake()
    {
        base.Awake();

        //find the physics object somewhere in the hierarchy of object
        myPhysObj = GetComponent<PhysicsObject>();
        if(myPhysObj == null) { myPhysObj = GetComponentInParent<PhysicsObject>(); }
        if (myPhysObj == null) { myPhysObj = GetComponentInChildren<PhysicsObject>(); }
    }

    private void Start()
    {
        if(playerPhysObj == null)
        {
            playerPhysObj = Player.GetComponent<PhysicsObject>();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        //check if in contact with the player and player is interacting 
        if (playerTouching)
        {
            if (Input.GetKeyDown(triggers[0]) || Input.GetKeyDown(triggers[1]) || Input.GetMouseButtonDown(0))
            {
                SetFrozen(true);
                Player.GetComponent<PhysicsObject>().InputVelocity = Vector2.zero;
                CheckQuest();
                if (questCompleted)
                {
                    if (dialogueBox.FirstChunk) { CallAfter(); }
                    if (dialogueBox.OnTriggerKeyPressed(completedDialogue, faceImage))
                    {
                        SetFrozen(false);
                    }
                }
                else
                {
                    if (dialogueBox.FirstChunk) { CallBefore(); } //only trigger event during the first chunk
                    if (dialogueBox.OnTriggerKeyPressed(questDialogue, faceImage))
                    {
                        SetFrozen(false);
                    }
                }
            }
            //exit the dialogue if the player jumps out
            else if (Input.GetButtonDown("Jump"))
            {
                dialogueBox.ExitReset();
            }
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

    protected override void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //exit dialogue when player leaves
        {
            indicator.SetActive(false);
            playerTouching = false;

            //dialogueBox.Reset(); //reset the dialogue when the player leaves
            if (disableAfter) { gameObject.SetActive(false); }
        }
    }
}
