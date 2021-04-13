using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorsNote : Button
{
    protected override void OnActive() {
    }

    protected override void OnClick()
    {
        Pause.otherPause = false;
        gameObject.SetActive(false);
    }

    protected override void OnEnter() { }
}

