using UnityEngine;

public class CollectableItem : Inventory
{
    private const float speed = 4.0f;
    private const float minMoveDistance = 0.05f;

    private bool collected = false;
    private bool animate = false;

    private Vector3 targetPositionOffset = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;
    private Vector2 pickupPosition;
    private Vector2 launch;

    public void Awake()
    {
        pickupPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //check if collision with player
        if (!collected && coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collected = true;
            AddItem(gameObject.name, gameObject); //add item to inventory and move to final location in UI

            targetPositionOffset = transform.position - MainCamera.transform.position; //determine tagetPosition relative to camera
            transform.position = pickupPosition; //return to origional position for animation
            animate = true; //start the animation
            targetPosition = MainCamera.transform.position + targetPositionOffset;
            launch = (new Vector3(targetPosition.x + Random.Range(-3, 3), targetPosition.y + Random.Range(-3, 3)) - transform.position).normalized * speed;
        }
    }

    //TODO:
    //Give it a Vector Launch that is within 30 degree range of the target position and gets weaker as time passes
    //Another vector always points towards the target position and gets stronger as the distance to target decreases

    /*
    private void Update()
    {
        if (animate) //TODO: make thread? Started by OnTriggerEnter
        {
            targetPosition = MainCamera.transform.position + targetPositionOffset;
            
            launch = launch.magnitude > 0.01f ? launch * 0.8f : Vector2.zero;
            Vector2 directVector = targetPosition - transform.position;
            if(directVector.magnitude < minMoveDistance)
            {
                transform.position = MainCamera.transform.position + targetPositionOffset; //snap into position
                Destroy(this); //remove collectable item script from item
            }
            directVector = directVector.normalized * (speed / directVector.magnitude);
            transform.position += new Vector3(directVector.x + launch.x, directVector.y + launch.y);
        }
    }
    */
    
    private void Update()
    {
        if (animate) //TODO: make thread? Started by OnTriggerEnter
        {
            //check if close enought to snap into position
            if ((targetPosition - transform.position).magnitude <= minMoveDistance)
            {
                transform.position = MainCamera.transform.position + targetPositionOffset; //snap into position
                Destroy(this); //remove collectable item script from item
            }
            Vector3 move = new Vector3(Mathf.Lerp(transform.position.x, targetPosition.x, speed * Time.deltaTime),                    //Lerp to x position
                                                         Mathf.Lerp(transform.position.y, targetPosition.y, speed * Time.deltaTime ), //Lerp to y position
                                                         transform.position.z);                                                       //Maintain z position

            transform.position = move.magnitude > minMoveDistance ? move : move.normalized * minMoveDistance; 
            targetPosition = MainCamera.transform.position + targetPositionOffset; //update the target for camera movemet
        }
    }
    
}
