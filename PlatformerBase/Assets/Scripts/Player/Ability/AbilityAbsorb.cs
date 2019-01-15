using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAbsorb : BloodParticle
{
    [SerializeField] private int particlesSent;

    //accessor for audioPlayer (used by Ability Handler)
    public AudioPlayer AudioPlayer { get { return audioPlayer; } }

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        audioPlayer = GetComponent<AudioPlayer>();
    }



    // Update is called once per frame
    void Update()
    {
        if(!paused)
        {
            //snap to players position
            transform.position = Player.transform.position;

            //move the particles toward their target
            MoveParticles();

            if (Input.GetMouseButton(1))
            {
                SendParticlesTo(Player.transform, particlesSent);
                part.Play();
            }
        }
        else
        {
            part.Pause();
        }
    }
}
