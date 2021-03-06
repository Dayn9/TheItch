﻿using System.Collections.Generic;
using UnityEngine;

public delegate void triggered();

[RequireComponent(typeof(Collider2D))]
public class EventTrigger : Inventory, ILevelData
{
    [SerializeField] protected bool disableAfter;

    protected bool playerTouching = false; //true when dialogue is touching Charachter

    [Header("--- Items Required ---")]
    [SerializeField] protected List<GameObject> itemsRequired; //items required to complete quest
    [SerializeField] protected bool itemsEaten; //true when required items are taken when triggered
    [SerializeField] private int abilityRequired = 0; 

    [Header("--- Item Given ---")]
    [SerializeField] protected List<GameObject> itemsGiven; //item given to the player
    [SerializeField] private bool givenOnComplete; //true = given on quest complete, false = given on initial interaction
    [SerializeField] private bool indirect = false;
    [SerializeField] protected bool givenImmediatly = true; //Last minute addition for edge case that I despise but...

    protected bool questCompleted = false; //true when quest has been completed

    public event triggered Before; //Event Triggered on player iteraction when quest incomplete
    public event triggered After; //Even Triggered on player interaction when quest complete

    protected AudioPlayer audioPlayer; //ref to the attached audio player

    public virtual bool State { get { return questCompleted; } }
    public string Name { get { return gameObject.name; } }

    protected virtual void Awake () {
        gameObject.GetComponent<Collider2D>().isTrigger = true;

        //insure there is never a null event
        Before = new triggered(NullEvent);
        After = new triggered(NullEvent);

        audioPlayer = GetComponentInParent<AudioPlayer>();

        if (itemsGiven.Count > 0 && !indirect) { itemsGiven.ForEach(i => i.transform.localPosition = transform.localPosition); }
    }


    /// <summary>
    /// default event so null ref errors are not thrown
    /// </summary>
    protected void NullEvent() { }

    public virtual void OnLevelLoad(bool state)
    {
        questCompleted = state;
        if (itemsEaten)
        {
            foreach(GameObject item in itemsRequired)
            {
                //check for Gem items when quest completed
                CollectableItem collItem = item.GetComponent<CollectableItem>();
                if (questCompleted && collItem != null && collItem.IsGem)
                {
                    GameObject gem = Instantiate(item);
                    gem.transform.position = transform.position;
                    gem.GetComponent<CollectableItem>().Eaten(transform);
                }
            }
        }
    }

    
    private void OnEnable()
    {
        //don't active the item given when this object activates
        if (itemsGiven.Count > 0) {
            itemsGiven.ForEach(i => i.SetActive(false));
        }
    }

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

    /// <summary>
    /// used by child classes to call Before Event
    /// </summary>
    protected void CallBefore()
    {
        Before();
    }
    /// <summary>
    /// used by shild classes to call After Event
    /// </summary>
    protected void CallAfter()
    {
        After();
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) //trigger dialogue when player touches 
        {
            playerTouching = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) //exit dialogue when player leaves
        {
            playerTouching = false;
            if (disableAfter) { gameObject.SetActive(false); }
        }
    }

    /// <summary>
    /// test if conditions are met for quest to be completed
    /// </summary>
    protected void CheckQuest()
    {
        if (!questCompleted) //only check for completion when incomplete
        {
            questCompleted = CheckItems() && CheckAbility();

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.T)) { questCompleted = true; }
#endif
            //give the items to player
            if (givenImmediatly)
            {
                GiveItems();
            }
        }

        //if (audioPlayer != null) { audioPlayer.PlaySound(questCompleted ? 1 : 0); } //play audio bsed on quest completion
    }

    /// <summary>
    /// spawns all given items when appropriate and breaks references
    /// </summary>
    protected void GiveItems()
    {
        if((givenOnComplete && questCompleted || !givenOnComplete) && itemsGiven.Count > 0 && itemsGiven[0].activeSelf == false){
            foreach (GameObject itemGiven in itemsGiven)
            {
                if (itemGiven && itemGiven.activeSelf == false)
                {
                    itemGiven.SetActive(true);
                    if (!indirect)
                    {
                        itemGiven.transform.position = Player.transform.position;
                    }
                }
            }
            itemsGiven.Clear(); //break reference 
        }
    }

    /// <summary>
    /// check if the required ability has been unlocked
    /// </summary>
    /// <returns></returns>
    private bool CheckAbility()
    {
        return abilityRequired < 0 ? true : AbilityHandler.Unlocked[abilityRequired];
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

    protected bool CheckInput()
    {
        return Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.S) ||
                Input.GetMouseButtonDown(0) ||
                Input.GetMouseButtonDown(1) ||
                Input.GetButtonDown("Interact");
    }
}
