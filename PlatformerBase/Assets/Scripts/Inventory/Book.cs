using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Button
{
    private bool unlocked = false;

    [SerializeField] [Range(0, 1)] private float normalAlpha;
    [SerializeField] [Range(0, 1)] private float lockedAlpha;
    [SerializeField] [Range(0, 1)] private float highlightAlpha;

    private Color normalColor;
    private Color lockedColor;
    private Color highlightColor;

    protected override void Awake()
    {
        base.Awake();

        //color setup
        normalColor = new Color(1, 1, 1, normalAlpha);
        lockedColor = new Color(1, 1, 1, lockedAlpha);
        highlightColor = new Color(1, 1, 1, highlightAlpha);

        render.color = lockedColor;
    }

    public void Unlock()
    {
        unlocked = true;
        render.color = normalColor;
    }

    protected override void OnActive()
    {
        render.color = unlocked ? normalColor : lockedColor;
    }

    protected override void OnEnter()
    {
        if (unlocked)
        {
            render.color = highlightColor;
        }
    }

    protected override void OnClick()
    {
        Debug.Log("Joined a Book Club");
    }
}
