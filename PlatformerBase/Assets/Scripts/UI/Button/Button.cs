using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Button : Pause {

    protected SpriteRenderer render; //ref to spriteRenderer component
    [SerializeField] protected Vector2 offset; //offset of collider from transform center
    [SerializeField] protected Rect area; //bounding shape of the button, never gets adjusted 

    private Vector2 pos; //position of the button with offset
    private Rect bounds; //actual bounds of the button

    [SerializeField] protected Sprite active; //sprite to display when mouse over
    [SerializeField] protected Sprite inactive; //sprite to display when mouse not over

    [SerializeField] private bool requiresPause = true;


    protected virtual void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        //set the area based on starting position and offset
        pos = (Vector2)transform.position + offset;

        GetAudioPlayer();
    }

    protected void Update () {
        if (paused || !requiresPause)
        {
            OnActive();
            //update the collider position
            pos = (Vector2)transform.position + offset;
            bounds = new Rect(pos.x + area.x, pos.y + area.y, area.width, area.height);

            //check if the mouse is withing the collider space
            if (bounds.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)))
            {
                
                OnEnter();
                //check if clicking (left mouse button)
                if (Input.GetMouseButtonDown(0))
                {
                    audioPlayer.PlaySound(0);
                    OnClick();
                }
            }
        }
	}

    /// <summary>
    /// called when menuUI is active
    /// </summary>
    protected abstract void OnActive();
    /// <summary>
    /// called when mouse enters button area
    /// </summary>
    protected abstract void OnEnter();
    /// <summary>
    /// called when button is clicked
    /// </summary>
    protected abstract void OnClick();

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
