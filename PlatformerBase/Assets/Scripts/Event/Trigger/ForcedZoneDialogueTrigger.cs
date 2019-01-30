﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedZoneDialogueTrigger : ZoneDialogueTrigger
{
    /// <summary>
    /// Forces the player to move to dialoge object while triggering dialogue
    /// </summary>

    [SerializeField] private float inputSpeed; //speed to give the player when moving towards dialogue
    [SerializeField] private float distanceAway; //distance away from the object to move o 

    private GameObject after; //gameobject to activate when dialogue ends (should contain regular dialogue trigger)

    private bool talked = false; //true once player has already interacted with dialogue object

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
                //send the dialogue and check if ended
                if (dialogueBox.OnTriggerKeyPressed(enterDialogue))
                {
                    //unfreeze player and activate after object
                    Player.GetComponent<IPlayer>().Frozen = false;
                    Player.GetComponent<PhysicsObject>().InputVelocity = Vector2.zero;
                    after.SetActive(true);
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
            dialogueBox.OnTriggerKeyPressed(enterDialogue);
            Player.GetComponent<IPlayer>().Frozen = true;
        }
    }

}