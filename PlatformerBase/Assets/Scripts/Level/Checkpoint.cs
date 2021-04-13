using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Checkpoint : MonoBehaviour {

    /// <summary>
    /// Changes the player's reset position
    /// </summary>

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true; //make sure the collider is a trigger
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //set the reset position of the player to this objects location
        if(coll.tag == "Player") {
            Global.Player.GetComponent<IPlayer>().ReturnPosition = transform.position;
        }
    }
}
