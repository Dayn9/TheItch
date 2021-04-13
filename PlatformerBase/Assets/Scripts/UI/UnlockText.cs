using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockText : MonoBehaviour {

    [SerializeField] private Vector2 hiddenOffset; //relative position of display when hidden
    [SerializeField] private float moveSpeed; //how fast to hide and show

    [SerializeField] private float timeActive;
    private float timer;

    private bool hidden; //true when inventory is empty and display not showing
    private Vector2 moveVector; //temporary vector when moving 
    private Vector3 origin;

    private SpriteRenderer render;

    private void Awake()
    {
        timer = timeActive;
        hidden = true;
        render = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
        origin = transform.localPosition;
        transform.localPosition += (Vector3)hiddenOffset;
    }
	
	// Update is called once per frame
	void Update () {
        if (!Global.paused)
        {
            if(timer < timeActive)
            {
                if (hidden) { MoveToLocalPosition(origin); }
                timer += Time.deltaTime;
            }else if (!hidden)
            {
                MoveToLocalPosition(origin + (Vector3)hiddenOffset);
            }
        }
	}

    public void ShowText()
    {
        render.enabled = true;
        timer = 0;
    }

    /// <summary>
    /// Moves the inventoryDisplay to a target location local to inventory
    /// </summary>
    /// <param name="target">position to move to</param>
    private void MoveToLocalPosition(Vector3 target)
    {
        moveVector = (target - transform.localPosition).normalized * moveSpeed * Time.deltaTime;
        //check if close enought to snap into position
        if ((target - transform.localPosition).magnitude < moveVector.magnitude)
        {
            transform.localPosition = target; //snap into position
            hidden = !hidden; //toggle hidden
            return; //stop moving
        }
        transform.localPosition += (Vector3)moveVector;
    }
}
