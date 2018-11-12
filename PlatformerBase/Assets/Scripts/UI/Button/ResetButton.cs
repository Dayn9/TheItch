using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Reseter))]
public class ResetButton : Button {

    protected override void OnActive()
    {
        buttonRender.sprite = inactive;
    }

    protected override void OnClick()
    {
        GetComponent<Reseter>().ResetGame();
    }

    protected override void OnEnter()
    {
        buttonRender.sprite = active;
    }
}
