using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatAudioPlayer : AudioPlayer {

    [SerializeField] private SoundFile heartbeat; //heartbeat sound effect

    //adjust the pitch and volume to match the desired speed [0, 10/3]
    public float Speed {
        set {
            source.pitch = value; //pitch [0, 10/3]
            source.volume = 0.9f - ((value * 0.8f) / (10 / 3.0f)); //volume [0.1, 0.9]
        }
    }

	void Start () {
        //insure the corret heartbeat clip is repeating
        source.clip = heartbeat.Clip;
        source.loop = true;
        source.Play();
	}
	
}
