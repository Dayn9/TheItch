using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider2D))]
public class FallZone : Global {

    private Transform[] fallSections;
    [SerializeField] private Rect zone; // zone should be greater than camera zone

    private Vector2 velocity = Vector2.zero;
    private float verticalPosOnContact; // y coordinate of the player when they entered the trigger of the fall zone
    private bool playerInZone = false;

    private Transform topFallSection;
    private Transform bottomFallSection;
    private int fallSectionsIndex;

    private bool brake = false;

	// Use this for initialization
	void Awake () {
        //make sure ther is 
        Assert.IsTrue(transform.childCount > 1, "Fall section must contain at least 2 fall sections");
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
        topFallSection = fallSections[0];
        bottomFallSection = fallSections[1];
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!paused && playerInZone)
        {
            topFallSection.position += (Vector3)velocity;
            bottomFallSection.position += (Vector3)velocity;

            if (topFallSection.localPosition.y >= zone.height)
            {
                //reset the top section
                topFallSection.localPosition = Vector3.down * zone.height;
                topFallSection.gameObject.SetActive(false);

                //break the loop
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

                //start the bottom section
                bottomFallSection.localPosition = topFallSection.localPosition + (Vector3.down * zone.height);
                bottomFallSection.gameObject.SetActive(true);
                bottomFallSection.position += (Vector3)velocity;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                brake = true;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) //trigger dialogue when player touches 
        {
            verticalPosOnContact = coll.transform.position.y;
            playerInZone = true;
            Player.GetComponent<IPlayer>().InFallZone = true;
            velocity = -Player.GetComponent<PhysicsObject>().GravityVelocity;
            bottomFallSection.gameObject.SetActive(true);
            fallSectionsIndex = 1;
            brake = false;
        }
    }

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
