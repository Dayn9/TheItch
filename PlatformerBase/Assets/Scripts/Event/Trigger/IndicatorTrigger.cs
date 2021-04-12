using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorTrigger : EventTrigger
{
    [SerializeField] protected GameObject indicatorPrefab; //object that appears when the player can interact with object
    [SerializeField] protected Vector3 indicatorOffset; //offset from door to display above object
    protected GameObject indicator; //ref to instatiated indicatorPrefab

    protected override void Awake()
    {
        //create the indicator and position it propperly 
        indicator = indicatorPrefab != null ? Instantiate(indicatorPrefab, transform) : new GameObject();

        indicator.transform.position = transform.position + indicatorOffset;
        indicator.SetActive(false);
        indicator.name = "Event Indicator";

        base.Awake();
    } 

    protected override void Update()
    {
        if (!paused)
        {
            //check if in contact with the player and player is interacting 
            if (playerTouching && CheckInput())
            {
                CheckQuest();
                if (questCompleted)
                {
                    CallAfter();
                    audioPlayer.PlaySound(1);
                }
                else
                {
                    CallBefore();
                    audioPlayer.PlaySound(0);
                }
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            indicator.SetActive(true);
            playerTouching = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //exit dialogue when player leaves
        {
            indicator.SetActive(false);
            playerTouching = false;
            if (disableAfter) { gameObject.SetActive(false); }
        }
    }


}
