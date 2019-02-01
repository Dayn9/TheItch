using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : IndicatorTrigger {
    [SerializeField] private DialogueBox dialogueBox;

    [Tooltip("img should be 32x32 pixels")]
    [SerializeField] private Sprite faceImage; //image to display in dialogue box

    [SerializeField] [TextArea] private string QuestDialogue; //text dialogue to give quest
    [SerializeField] [TextArea] private string CompletedDialogue; //text dialogue when quest complete2

    private PhysicsObject myPhysObj;
    private static PhysicsObject playerPhysObj;

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
                setFrozen(true);
                CheckQuest();
                if (questCompleted)
                {
                    if (dialogueBox.FirstChunk) { CallAfter(); }
                    if (dialogueBox.OnTriggerKeyPressed(CompletedDialogue, faceImage))
                    {
                        setFrozen(false);
                    }
                }
                else
                {
                    if (dialogueBox.FirstChunk) { CallBefore(); } //only trigger event during the first chunk
                    if (dialogueBox.OnTriggerKeyPressed(QuestDialogue, faceImage))
                    {
                        setFrozen(false);
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
    private void setFrozen(bool frozen)
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
