using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatAudioPlayer : AudioPlayer
{

    [SerializeField] private SoundFile heartbeat; //heartbeat sound effect

    //adjust the pitch and volume to match the desired speed [0, 10/3]
    public float Speed
    {
        set
        {
            source.pitch = value; //pitch [0, 10/3]
            source.volume = 0.5f - ((value * 0.4f) / (10 / 3.0f)); //volume [0.1, 0.9]
        }
    }

    public float Volume
    {
        get { return source.volume; }
        set { source.volume = value; }
    }

    void Start()
    {
        //insure the corret heartbeat clip is repeating
        currentPriority = heartbeat.Priority;
        source.clip = heartbeat.Clip;
        source.loop = true;
        source.Play();
    }

    protected override void Update()
    {
        base.Update();
        if (Global.paused) { source.Pause(); }
        else if (!source.isPlaying) { source.Play(); }
    }
}
