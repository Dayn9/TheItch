using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLabel : Button
{
    private int numChars = 3; //number of characters in this item label 
    private const float maxWidth = 5; //width of the sprite
    private float hiddenWidth; //width of the sprite when not visible

    private float targetWidth; //width of the sprite being lerped to

    private int slotNum = 0;
    public int SlotNum { set { slotNum = value; } }

    private void Start()
    {
        hiddenWidth = ((maxWidth * pixelsPerUnit) - (numChars * 6) - 1) / pixelsPerUnit;
        render.size = new Vector2(hiddenWidth, render.size.y);
    }

    protected override void OnActive()
    {
        targetWidth = hiddenWidth;
    }
    protected override void OnClick() { }

    protected override void OnEnter()
    {
        targetWidth = maxWidth;
    }

    protected new void Update()
    {
        base.Update();
        render.size = new Vector2(Mathf.Lerp(render.size.x, targetWidth, 0.3f), 
                                  render.size.y);
    }
}
