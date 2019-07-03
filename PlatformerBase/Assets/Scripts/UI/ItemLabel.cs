using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemLabel : Inventory
{
    protected SpriteRenderer render; //ref to spriteRenderer component
    [SerializeField] protected Vector2 offset; //offset of collider from transform center
    [SerializeField] protected Rect area; //bounding shape of the button, never gets adjusted 

    private Vector2 pos; //position of the button with offset
    private Rect bounds; //actual bounds of the button

    private int numChars = 3; //number of characters in this item label 
    private const float maxWidth = 5; //width of the sprite
    private float hiddenWidth; //width of the sprite when not visible

    private float targetWidth; //width of the sprite being lerped to

    [SerializeField] private Sprite[] labels; //key, gem, lily, skull, letter
    [SerializeField] private Color[] styles;

    private int slotNum = 0;
    public int SlotNum { set { slotNum = value; } }

    [SerializeField] private Material styleMat;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        //set the area based on starting position and offset
        pos = (Vector2)transform.position + offset;

        render.material = styleMat;
        render.material.SetColor("_ReplaceColor", styles[0]);

        SetHiddenWidth();
    }

    private void SetHiddenWidth()
    {
        hiddenWidth = ((maxWidth * pixelsPerUnit) - (numChars * 6) - 1) / pixelsPerUnit;
        render.size = new Vector2(hiddenWidth, render.size.y);
    }

    protected void Update()
    {
        if (!paused)
        {
            //update the collider position
            pos = (Vector2)transform.position + offset;
            bounds = new Rect(pos.x + area.x, pos.y + area.y, area.width, area.height);

            if(Items.Count > slotNum)
            {
                //set the width
                targetWidth = bounds.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition))
                    ? maxWidth : hiddenWidth;
            }
            else
            {
                targetWidth = 0;
            }

            render.size = new Vector2(Mathf.Lerp(render.size.x, targetWidth, 0.3f), render.size.y);
        }
    }

    public void SetLabel(ItemType type, ItemStyle style)
    {
        //set the text to match item type
        switch (type)
        {
            case ItemType.Key:
                render.sprite = labels[0];
                numChars = 3;
                break;
            case ItemType.Gem:
                render.sprite = labels[1];
                numChars = 3;
                break;
            case ItemType.Lily:
                render.sprite = labels[2];
                numChars = 4;
                break;
            case ItemType.Skull:
                render.sprite = labels[3];
                numChars = 5;
                break;
            case ItemType.Letter:
                render.sprite = labels[4];
                numChars = 6;
                break;
            case ItemType.Cure:
                render.sprite = labels[5];
                numChars = 4;
                break;
        }

        //set the color to match item style
        switch (style)
        {
            default:
            case ItemStyle.Default:
                render.material.SetColor("_ReplaceColor", styles[0]);
                break;
            case ItemStyle.Blood:
                render.material.SetColor("_ReplaceColor", styles[1]);
                break;
            case ItemStyle.Heal:
                render.material.SetColor("_ReplaceColor", styles[2]);
                break;
            case ItemStyle.Water:
                render.material.SetColor("_ReplaceColor", styles[3]);
                break;
            case ItemStyle.Virus:
                render.material.SetColor("_ReplaceColor", styles[4]);
                break;
        }

        SetHiddenWidth();
    }

    /// <summary>
    /// draw a green collider based on area
    /// </summary>
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

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
