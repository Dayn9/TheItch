using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedZoneDialogueTrigger : ZoneDialogueTrigger
{
    [SerializeField] private float inputSpeed;

    private GameObject after;

    private bool talked = false;

    protected override void Awake()
    {
        base.Awake();

        after = transform.GetChild(0).gameObject;
        after.SetActive(false);
    }

    protected override void Update()
    {
        if (playerTouching && !talked)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetMouseButtonDown(0))
            {
                if (dialogueBox.OnTriggerKeyPressed(enterDialogue))
                {
                    Player.GetComponent<IPlayer>().Frozen = false;
                    after.SetActive(true);
                    talked = true;
                }
            }

            if ((transform.position - Player.transform.position).magnitude > 2)
            {
                Player.GetComponent<PhysicsObject>().InputVelocity = new Vector2(Player.transform.position.x > transform.position.x ? -inputSpeed : inputSpeed, 0);
            }
            else
            {
                Player.GetComponent<PhysicsObject>().InputVelocity = Vector2.zero;
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (!talked && coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            CallBefore();
            playerTouching = true;
            dialogueBox.Reset(); //make sure the dialogue box is wipeed
            dialogueBox.OnTriggerKeyPressed(enterDialogue);
            Player.GetComponent<IPlayer>().Frozen = true;
        }
    }

}
