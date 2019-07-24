using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : Button { 

    [SerializeField] private Sprite mActive;
    [SerializeField] private Sprite mInactive;


    protected override void OnActive()
    {
        render.sprite = muted ? mInactive : inactive;
    }

    protected override void OnClick()
    {
        muted = !muted;
    }

    protected override void OnEnter()
    {
        render.sprite = muted ? mActive : active;
    }
   
}
