using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DontDestroy))] //background audio player shouldn't be destroyed
public class BackgroundAudioPlayer : AudioPlayer
{
    private int soundP = 0;

    public static bool menu = true;

    private void Start()
    {
        if(soundP == 0)
        {
            soundP = 2;
            PlaySound(0);
        }
    }

    private void Update()
    {
        if (!menu)
        {
            float bpm = Heartbeat.BPM;
            if (bpm <= 32)
            {
                if (soundP != 2)
                {
                    soundP = 2;
                    PlaySoundOverride(2);
                }
            }
            else if (bpm >= 168)
            {
                if (soundP != 3)
                {
                    soundP = 3;
                    PlaySoundOverride(3);
                }
            }
            else
            {
                if (soundP != 1)
                {
                    soundP = 1;
                    PlaySoundOverride(1);
                }
            }
        }
    } 
}
