using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Books : MonoBehaviour
{
    [SerializeField] private Vector2 hiddenOffset; //relative position of display when hidden
    private Vector2 openOffset; 
    [SerializeField] private float moveSpeed; //how fast to hide and show

    private bool hidden = true; //true when inventory is empty and display not showing
    private Vector2 moveVector; //temporary vector when moving 

    [SerializeField] private Digit collectedDigit;
    [SerializeField] private Digit totalDigit;
    private int collectedPages = 0;
    private int totalPages;

    private const float displayTime = 3.0f;
    private float timer = 0;

    private void Update()
    {
        if (!Pause.menuPaused) //TODO
        {
            //display is hidden but there are items
            if (hidden && timer < displayTime)
            {
                MoveToLocalPosition(openOffset);
            }
            //display is showing but there are no items
            else if (!hidden && timer > displayTime)
            {
                MoveToLocalPosition(hiddenOffset);
            }
            timer += Time.deltaTime;
        }
        collectedDigit.SetNumber(collectedPages);


        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 10); //TODO delete this garbage
    }

    private void Awake()
    {
        totalPages = FindObjectsOfType<Page>().Length;
        collectedPages = 0;
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
        timer = 0;
    }

    /// <summary>
    /// Moves the inventoryDisplay to a target location local to inventory
    /// </summary>
    /// <param name="target">position to move to</param>
    private void MoveToLocalPosition(Vector2 target)
    {
        moveVector = (((Vector3)target) - transform.localPosition).normalized * moveSpeed * Time.deltaTime;
        //check if close enought to snap into position
        if ((((Vector3)target) - transform.localPosition).magnitude < moveVector.magnitude)
        {
            transform.localPosition = target; //snap into position
            hidden = !hidden; //toggle hidden
            return; //stop moving
        }
        transform.localPosition += (Vector3)moveVector;
    }
}
