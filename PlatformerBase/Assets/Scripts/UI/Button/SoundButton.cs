using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : Button { 

    [SerializeField] private Sprite mActive;
    [SerializeField] private Sprite mInactive;


    protected override void OnActive()
    {
        render.sprite = Global.muted ? mInactive : inactive;
    }

    protected override void OnClick()
    {
        Global.muted = !Global.muted;
    }

    protected override void OnEnter()
    {
        render.sprite = Global.muted ? mActive : active;
    }
   
}
