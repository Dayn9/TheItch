using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButton : Button {

    [SerializeField] private Sprite mActive;
    [SerializeField] private Sprite mInactive;


    protected override void OnActive()
    {
        buttonRender.sprite = mute ? mInactive : inactive;
    }

    protected override void OnClick()
    {
        mute = !mute;
        foreach (AudioSource source in audios)
        {
            source.mute = mute;
        }
    }

    protected override void OnEnter()
    {
        buttonRender.sprite = mute ? mActive : active;
    }
   
}
