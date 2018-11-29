﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void triggered();

[RequireComponent(typeof(Collider2D))]
public class EventTrigger : Inventory {
    protected bool playerTouching = false; //true when dialogue is touching Charachter

    [SerializeField] protected List<GameObject> itemsRequired; //items required to complete quest
    [SerializeField] protected bool itemsEaten; //true when required items are taken when triggered
    protected bool questCompleted = false; //true when quest has been completed

    public event triggered Before; //Event Triggered on player iteraction when quest incomplete
    public event triggered After; //Even Triggered on player interaction when quest complete

    protected AudioPlayer audioPlayer;

    protected virtual void Awake () {
        gameObject.GetComponent<Collider2D>().isTrigger = true;

        //insure there is never a null event
        Before = new triggered(NullEvent);
        After = new triggered(NullEvent);

        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    /// <summary>
    /// default event so null ref errors are not thrown
    /// </summary>
    protected void NullEvent() { }

    protected virtual void Update()
    {
        if (!paused)
        {
            //check if in contact with the player and player is interacting 
            if (playerTouching)
            {
                CheckQuest();
                if (questCompleted)
                {
                    After();
                }
                else
                {
                    Before();
                }
            }
        }
    }

    protected void CallBefore()
    {
        Before();
    }

    protected void CallAfter()
    {
        After();
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            playerTouching = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //exit dialogue when player leaves
        {
            playerTouching = false;
        }
    }

    /// <summary>
    /// test if conditions are met for quest to be completed
    /// </summary>
    protected void CheckQuest()
    {
        
        if (!questCompleted) //only check for completion when incomplete
        {
            
            questCompleted = CheckItems();
            if (!questCompleted && audioPlayer != null) {

                audioPlayer.PlaySound(0); //play the null Sound
            }
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
            if (item == null || !Items.ContainsKey(item.name))
            {                
                return false; //a required item is missing from player inventory or gone
            }
        }
        //remove the items from inventory if necissary
        if (itemsEaten)
        {
            foreach (GameObject item in itemsRequired)
            {
                RemoveItem(item.name);
            }
        }
        return true; //all required items are in the players inventory
    }
}
