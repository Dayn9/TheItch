using UnityEngine;
using System.Linq;

public class ForcedZoneDialogueTrigger : ZoneDialogueTrigger
{
    /// <summary>
    /// Forces the player to move to dialoge object while triggering dialogue
    /// </summary>

    [SerializeField] private float inputSpeed; //speed to give the player when moving towards dialogue
    [SerializeField] private float distanceAway; //distance away from the object to move o 

    private GameObject after; //gameobject to activate when dialogue ends (should contain regular dialogue trigger

    private bool talked = false; //true once player has already interacted with dialogue object

    protected override void Start()
    {
        base.Start();
        //get reference to gameobject to activate after mandatory dialogue
        if (after == null && transform.childCount > 0)
        {
            after = transform.GetChild(0).gameObject;
            after.SetActive(talked);
        }
    }

    public override bool State { get { return talked; } }
    public override void OnLevelLoad(bool state)
    {
        talked = state;
    }

    protected override void Update()
    {
        if (playerTouching && !talked && !Pause.menuPaused)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetMouseButtonDown(0))
            {
                if (personIndex >= 0 && DialogueBox.PeopleTalked[personIndex] == false)
                {
                    DialogueBox.PeopleTalked[personIndex] = true;
                    
                    if (!DialogueBox.PeopleTalked.ToList().Contains(false))
                    {
                        
                    }
                } 
                CheckQuest();
                //send the dialogue and check if ended
                if (dialogueBox.OnTriggerKeyPressed(questCompleted ? completedDialogue : enterDialogue, faceImage))
                {
                    //unfreeze player and activate after object
                    SetFrozen(false);
                    Player.GetComponent<PhysicsObject>().InputVelocity = Vector2.zero;
                    if (after != null)
                    {
                        after.SetActive(true);
                    }
                    talked = true;
                }
            }
            //move towards dialogue object
            if ((transform.position - Player.transform.position).magnitude > distanceAway)
            {
                Player.GetComponent<PhysicsObject>().InputVelocity = new Vector2(Player.transform.position.x > transform.position.x ? -inputSpeed : inputSpeed, 0);
            }
            //close enought to dialogue object
            else
            {
                Player.GetComponent<PhysicsObject>().InputVelocity = Vector2.zero;
            }
        }
    }

    //override used to include the talked bool
    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (!talked && coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            CallBefore();
            playerTouching = true;
            dialogueBox.Reset(); //make sure the dialogue box is wipeed

            //check for quest completion and display appropriate dialogue
            CheckQuest();
            dialogueBox.OnTriggerKeyPressed(questCompleted ? completedDialogue : enterDialogue, faceImage);

            SetFrozen(true);
        }
    }

}
