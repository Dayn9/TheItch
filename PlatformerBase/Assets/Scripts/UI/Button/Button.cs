using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Button : Pause {

    [SerializeField] protected Vector2 offset; //offset of collider from transform center
    protected const float radius = 1.5f; //half the width of the collider box

    protected SpriteRenderer buttonRender; //ref to spriteRenderer component
    protected Rect area; //bounds to the collider

    [SerializeField] protected Sprite active; //sprite to display when mouse over
    [SerializeField] protected Sprite inactive; //sprite to display when mouse not over

    protected void Awake()
    {
        buttonRender = GetComponent<SpriteRenderer>();
        //set the area based on starting position and offset
        Vector2 pos = (Vector2)transform.position + offset;
        area = new Rect(pos.x - radius, pos.y - radius, 2 * radius, 2 * radius);
    }

    protected void Update () {
        if (paused)
        {
            OnActive();
            //update the collider position
            area.x = transform.position.x + offset.x - radius;
            area.y = transform.position.y + offset.y - radius;
            //check if the mouse is withing the collider space
            if (area.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)))
            {
                OnEnter();
                //check if clicking (left mouse button)
                if (Input.GetMouseButtonDown(0))
                {
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

        Vector2 pos = (Vector2)transform.position + offset;

        Vector2 topLeft = new Vector2(pos.x - radius, pos.y + radius);
        Vector2 topRight = new Vector2(pos.x + radius, pos.y + radius);
        Vector2 bottomLeft = new Vector2(pos.x - radius, pos.y - radius);
        Vector2 bottomRight = new Vector2(pos.x + radius, pos.y - radius);

        //draw lines between corners
        Gizmos.DrawLine(topLeft, topRight); //top
        Gizmos.DrawLine(topRight, bottomRight); //right
        Gizmos.DrawLine(bottomRight, bottomLeft); //bottom
        Gizmos.DrawLine(bottomLeft, topLeft); //left
    }
}
