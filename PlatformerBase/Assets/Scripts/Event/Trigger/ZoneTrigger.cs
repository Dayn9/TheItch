﻿using UnityEngine;

public class ZoneTrigger : EventTrigger {

    protected override void Update() { }

    //call the before event when player enters zone
    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (!questCompleted && coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            CheckQuest(); //callecd for item given reasons (don't delete)
            if (questCompleted)
            {
                CallBefore();
            }
            //if (disableAfter) { gameObject.SetActive(false); } DONT 
        }
    }

    //call the after event when player exits zone
    protected override void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //exit dialogue when player leaves
        {
            CallAfter();
            if (disableAfter) { gameObject.SetActive(false); }
        }
    }
}
