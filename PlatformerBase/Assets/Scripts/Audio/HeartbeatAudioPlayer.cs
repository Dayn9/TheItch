using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatAudioPlayer : AudioPlayer {

    [SerializeField] private SoundFile heartbeat;

    private float speed = 0;

    public float Speed {
        set {
            source.pitch = value;
        }
    }

	// Use this for initialization
	void Start () {
        source.clip = heartbeat.Clip;
        source.loop = true;
	}
	
}
