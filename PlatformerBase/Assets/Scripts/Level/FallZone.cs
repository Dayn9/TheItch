using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider2D))]
public class FallZone : Global {

    [SerializeField] private EventTrigger evTrig; //eventTrigger 

    private Transform[] fallSections; //fall Sections that scroll up when player is in the fall zone
    [SerializeField] private Rect zone; // zone should be greater than camera zone

    private Vector2 velocity = Vector2.zero; //velocity of the fallZone sections
    private bool playerInZone = false; //true when player enters the collider

    private Transform topFallSection; //ref to the active fall section that covers top part of screen
    private Transform bottomFallSection; //ref to the active fall section that covers bottom part of screen
    private int fallSectionsIndex; //index of the botttom fall section for looping through fallSections

    private bool brake = false; //true when fall sections should stop moving

	// Use this for initialization
	void Awake () {
        //make sure there are at least 2 fall sections available
        Assert.IsTrue(transform.childCount > 1, "Fall section must contain at least 2 fall sections");

        //find all the fall sections
        fallSections = new Transform[transform.childCount];
		for(int i = 0; i< transform.childCount; i++)
        {
            fallSections[i] = transform.GetChild(i);
            if(i == 0)
            {
                fallSections[i].gameObject.SetActive(true);
                
            }
            else
            {
                fallSections[i].gameObject.SetActive(false);
                fallSections[i].position += Vector3.down * zone.height;
            }
        }
        
        //select the first two fall sections to be displayed
        topFallSection = fallSections[0];
        bottomFallSection = fallSections[1];

        
    }

    private void Start()
    {
        evTrig.After += new triggered(Brake);
    }

    void FixedUpdate () {
		if(!paused && playerInZone)
        {
            //move the fall sections in direction of velocity
            topFallSection.position += (Vector3)velocity;
            bottomFallSection.position += (Vector3)velocity;

            //check if top fall section is no longer visible
            if (topFallSection.localPosition.y >= zone.height)
            {
                //reset the top section
                topFallSection.localPosition = Vector3.down * zone.height;
                topFallSection.gameObject.SetActive(false);

                //break the loop if necissary
                if (brake)
                {
                    bottomFallSection.localPosition = Vector2.zero;
                    playerInZone = false;
                    Player.GetComponent<IPlayer>().InFallZone = false;
                    return;
                }

                //resassign the fall sections
                fallSectionsIndex = (fallSectionsIndex + 1) % fallSections.Length;
                topFallSection = bottomFallSection;
                bottomFallSection = fallSections[fallSectionsIndex];

                //start the bottom fall section
                bottomFallSection.localPosition = topFallSection.localPosition + (Vector3.down * zone.height);
                bottomFallSection.gameObject.SetActive(true);
                bottomFallSection.position += (Vector3)velocity;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            //player stops moving
            playerInZone = true;
            Player.GetComponent<IPlayer>().InFallZone = true;

            //start the fall sections moving
            velocity = -Player.GetComponent<PhysicsObject>().GravityVelocity;
            bottomFallSection.gameObject.SetActive(true);
            fallSectionsIndex = 1;

            //reset the brake if needed
            brake = false;
        }
    }

    public void Brake() { brake = true; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector2 topLeft = new Vector2(transform.position.x + zone.x, transform.position.y + zone.y);
        Vector2 topRight = new Vector2(transform.position.x + zone.x + zone.width, transform.position.y + zone.y);
        Vector2 bottomLeft = new Vector2(transform.position.x + zone.x, transform.position.y + zone.y + zone.height);
        Vector2 bottomRight = new Vector2(transform.position.x + zone.x + zone.width, transform.position.y + zone.y + zone.height);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
