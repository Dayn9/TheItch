using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButton : Button
{
    protected override void OnActive()
    {
        buttonRender.sprite = inactive;
    }

    protected override void OnClick()
    {
        
    }

    protected override void OnEnter()
    {
        buttonRender.sprite = active;
    }
}
