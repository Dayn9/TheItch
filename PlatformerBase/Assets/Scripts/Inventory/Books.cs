using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Books : Global
{
    [SerializeField] private Vector2 hiddenOffset; //relative position of display when hidden
    private Vector2 openOffset; 
    [SerializeField] private float moveSpeed; //how fast to hide and show

    private bool hidden = false; //true when inventory is empty and display not showing
    private Vector2 moveVector; //temporary vector when moving 

    [SerializeField] private Digit collectedDigit;
    [SerializeField] private Digit totalDigit;
    private int collectedPages = 0;
    private int totalPages;

    private bool move = false;
    private const float displayTime = 3.0f;
    private float timer = 0;


    [SerializeField] protected Vector2 offset; //offset of collider from transform center
    [SerializeField] protected Rect area; //bounding shape of the button, never gets adjusted 
    private Vector2 pos; //position of the button with offset
    private Rect bounds; //actual bounds of the button

    private void Update()
    {
        if (!Pause.menuPaused) //TODO
        {
            //display is hidden but there are items
            if (hidden && move)
            {
                MoveToLocalPosition(openOffset);
            }
            //display is showing but there are no items
            else if (!hidden && move)
            {
                MoveToLocalPosition(hiddenOffset);
            }
            //not moving and display timer started
            else if(timer < displayTime)
            {
                timer += Time.deltaTime;
                if(timer > displayTime)
                {
                    move = true;
                }
            }

            //update the collider position
            pos = (Vector2)transform.position + offset;
            bounds = new Rect(pos.x + area.x, pos.y + area.y, area.width, area.height);
            //check if the mouse is withing the collider space
            if (hidden && bounds.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)))
            {
                timer = 0;
                move = true;
            }

        }
        collectedDigit.SetNumber(collectedPages);


        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 10); //TODO delete this garbage
    }

    private void Awake()
    {
        totalPages = FindObjectsOfType<Page>().Length;
        collectedPages = 0;

        //set the area based on starting position and offset
        pos = (Vector2)transform.position + offset;

        timer = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        openOffset = transform.localPosition;
        transform.localPosition = hidden ? hiddenOffset : openOffset; //move the display to a hidden position if hidden

        collectedDigit.SetNumber(collectedPages);
        totalDigit.SetNumber(totalPages);
    }

    public void CollectPage()
    {
        collectedPages++;
        if(collectedPages > totalPages)
        {

        }
        move = true;
        timer = 0;

    }

    /// <summary>
    /// Moves the inventoryDisplay to a target location local to inventory
    /// </summary>
    /// <param name="target">position to move to</param>
    private void MoveToLocalPosition(Vector2 target)
    {
        moveVector = (target - (Vector2)transform.localPosition).normalized * moveSpeed * Time.deltaTime;
       
        //check if close enought to snap into position
        if ((target - (Vector2)transform.localPosition).magnitude < moveVector.magnitude)
        {
            transform.localPosition = target; //snap into position
            hidden = !hidden; //toggle hidden
            
            move = false;
            return; //stop moving
        }
        transform.localPosition += (Vector3)moveVector;
    }

    /// <summary>
    /// draw a green collider based on area
    /// </summary>
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        pos = (Vector2)transform.position + offset;
        bounds = new Rect(pos.x + area.x, pos.y + area.y, area.width, area.height);

        Vector2 topLeft = new Vector2(bounds.x, bounds.y);
        Vector2 topRight = new Vector2(bounds.x + bounds.width, bounds.y);
        Vector2 bottomLeft = new Vector2(bounds.x, bounds.y + bounds.height);
        Vector2 bottomRight = new Vector2(bounds.x + bounds.width, bounds.y + bounds.height);

        //draw lines between corners
        Gizmos.DrawLine(topLeft, topRight); //top
        Gizmos.DrawLine(topRight, bottomRight); //right
        Gizmos.DrawLine(bottomRight, bottomLeft); //bottom
        Gizmos.DrawLine(bottomLeft, topLeft); //left
    }
}
