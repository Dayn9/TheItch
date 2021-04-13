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

    private AudioPlayer audioPlayer; //ref to the buildings AudioPlayer

    public int LayerInBuilding { set { layerInBuilding = value; } }

    protected override void Awake()
    {
        base.Awake();

        //create the indicator
        indicator = Instantiate(indicatorPrefab, transform);
        indicator.transform.position = transform.position + indicatorOffset;
        indicator.SetActive(false);
        indicator.name = "Door Indicator";

        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    private void Update()
    {
        bool interact = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetButtonDown("Interact");

        //trigger event when player is touching door and correct key is pressed
        if (!Global.paused && playerTouching && interact)
        {
            OnDoorOpen?.Invoke(layerInBuilding); //trigger event
            audioPlayer.PlaySound(0); //play the unlocking sound
        }
    }

    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            render.sprite = active;
            indicator.SetActive(true);
            playerTouching = true;
        }
        
    }

    protected override void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            render.sprite = inactive;
            indicator.SetActive(false);
            playerTouching = false;
        }
    }
}
