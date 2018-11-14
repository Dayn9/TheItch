using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : Button {

    [SerializeField] private Sprite mActive;
    [SerializeField] private Sprite mInactive;


    protected override void OnActive()
    {
        buttonRender.sprite = muted ? mInactive : inactive;
    }

    protected override void OnClick()
    {
        muted = !muted;
        foreach (AudioSource source in audios)
        {
            source.mute = muted;
        }
    }

    protected override void OnEnter()
    {
        buttonRender.sprite = muted ? mActive : active;
    }
   
}
