using UnityEngine;
using UnityEngine.Profiling;

[RequireComponent(typeof(SpriteRenderer))]
public class CollectableItem : Inventory
{
    private const float speed = 4.0f;
    private const float minMoveDistance = 0.05f;

    private bool collected = false;
    private bool used = false;
    private bool moving = false;

    private Vector3 targetPosition = Vector3.zero;
    private Vector2 pickupPosition;
    private Vector2 inventoryPosition;

    private SpriteRenderer render;

    public void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        pickupPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //check if collision with player
        if (!collected && coll.tag == "Player")
        {
            AddItem(gameObject.name, gameObject); //add item to inventory and move to final location in UI

            SpriteRenderer rend = GetComponent<SpriteRenderer>(); //make sure sorting layer and order is just above inventoryUI
            SpriteRenderer invRend = inventoryUI.GetChild(0).GetComponent<SpriteRenderer>();
            rend.sortingLayerID = invRend.sortingLayerID;
            rend.sortingOrder = invRend.sortingOrder + 1;

            targetPosition = transform.localPosition;
            transform.position = pickupPosition; //return to origional position for animation
            collected = true; //start the animation
            moving = true;

            collectEffect.transform.position = transform.position;
            collectEffect.SetActive(true);
            collectEffect.GetComponent<Animator>().SetTrigger("Collect");
        }
    }

    void Update()
    {
        if (!paused)
        {
            //Lerp into position
            if ((collected || used) && moving)
            {
                Vector2 move = new Vector2(Mathf.Lerp(transform.localPosition.x, targetPosition.x, speed * Time.deltaTime),
                                                             Mathf.Lerp(transform.localPosition.y, targetPosition.y, speed * Time.deltaTime));
                if ((targetPosition - transform.localPosition).magnitude <=  minMoveDistance)
                {
                    transform.localPosition = targetPosition; //snap into position
                    //if (used) { render.enabled = false; } 

                    moving = false;
                    return; //stop moving
                }
                //move at speed greater than min move speed
                transform.localPosition = move.magnitude > minMoveDistance ? move : move.normalized * minMoveDistance;
            }
        }
    }

    public void Eaten(Transform target)
    {
        transform.parent = target;
        targetPosition = Vector2.zero;

        SpriteRenderer targetRender = target.GetComponent<SpriteRenderer>();
        if (targetRender != null)
        {
            render.sortingLayerID = targetRender.sortingLayerID;
            render.sortingOrder = targetRender.sortingOrder - 1;
        }
        used = true;
        moving = true;
    }
}
