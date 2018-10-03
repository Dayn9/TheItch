using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// door opening delegate
/// </summary>
/// <param name="layer">layer the door is fully visible on</param>
/// <returns>returns true when door is open and false when door is closed</returns>
public delegate void DoorOpenMethod(int layer);

public class Door : Highlight {

    [SerializeField] private GameObject indicatorPrefab; //object that appears when the player can interact with door
    [SerializeField] private Vector3 indicatorOffset; //offset from door to display above object

    private GameObject indicator; //ref to instatiated indicatorPrefab
    private bool playerTouching = false; //true when player is touching door

    private int layerInBuilding; //layer where the door will be completly displayed
    public event DoorOpenMethod OnDoorOpen; //event triggered when door is interacted with

    public int LayerInBuilding { set { layerInBuilding = value; } }

    private void Start()
    {
        //create the indicator
        indicator = Instantiate(indicatorPrefab, transform);
        indicator.transform.position = transform.position + indicatorOffset;
        indicator.SetActive(false);
        indicator.name = "Door Indicator";
    }

    private void Update()
    {
        if (!paused)
        {
            //trigger event when player is touching door and correct key is pressed
            if (playerTouching && Input.GetKeyDown(KeyCode.X))
            {
                if (OnDoorOpen != null)
                {
                    OnDoorOpen(layerInBuilding);
                }
            }
        }
        
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        base.OnTriggerEnter2D(coll); //Highlight the object

        indicator.SetActive(true);
        playerTouching = true; 
    }

    protected override void OnTriggerExit2D(Collider2D coll)
    {
        base.OnTriggerExit2D(coll); //Stop Highlighting the object

        indicator.SetActive(false);
        playerTouching = false;
    }
}
