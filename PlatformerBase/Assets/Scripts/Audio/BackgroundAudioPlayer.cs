using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DontDestroy))] //background audio player shouldn't be destroyed
public class BackgroundAudioPlayer : AudioPlayer
{
    private static bool opening = true;

    private void Start()
    {
        PlaySound(0);
    }

    private void Update()
    {
        if(opening && !source.isPlaying)
        {
            PlaySound(1);
            source.loop = true;
            opening = false;
        }
    }

}
