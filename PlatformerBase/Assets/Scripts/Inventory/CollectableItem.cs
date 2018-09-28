using UnityEngine;

public class CollectableItem : Inventory
{
    private const float speed = 4.0f;
    private const float minMoveDistance = 0.05f;

    private bool collected = false;
    private bool inVentory = false;

    private Vector3 targetPosition = Vector3.zero;
    private Vector2 pickupPosition;

    public void Awake()
    {
        pickupPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //check if collision with player
        if (!collected && coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            AddItem(gameObject.name, gameObject); //add item to inventory and move to final location in UI

            SpriteRenderer rend = GetComponent<SpriteRenderer>(); //make sure sorting layer and order is just above inventoryUI
            SpriteRenderer invRend = inventoryUI.GetChild(0).GetComponent<SpriteRenderer>();
            rend.sortingLayerID = invRend.sortingLayerID;
            rend.sortingOrder = invRend.sortingOrder + 1;

            targetPosition = transform.localPosition;
            transform.position = pickupPosition; //return to origional position for animation
            collected = true; ; //start the animation
        }
    }
    
    private void Update()
    {
        //Lerp into position
        if (collected && !inVentory)
        {
            Vector2 move = new Vector2(Mathf.Lerp(transform.localPosition.x, targetPosition.x, speed * Time.deltaTime),                    
                                                         Mathf.Lerp(transform.localPosition.y, targetPosition.y, speed * Time.deltaTime));
            //check if close enought to snap into position
            if ((targetPosition - transform.localPosition).magnitude  < move.magnitude * minMoveDistance)
            {
                transform.localPosition = targetPosition; //snap into position
                inVentory = true;
                return; //stop moving
            }
            //move at speed greater than min move speed
            transform.localPosition = move.magnitude > minMoveDistance ? move : move.normalized * minMoveDistance;
        }
    }   
}
