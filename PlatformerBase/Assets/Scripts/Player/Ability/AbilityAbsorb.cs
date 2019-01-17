using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAbsorb : BloodParticle
{
    [SerializeField] private int particlesSent;
    private float particleEmission;

    private SpriteRenderer playerRend; //ref to players sprite renderer
    private MovingObject playerObj;

    //accessor for audioPlayer (used by Ability Handler)
    public AudioPlayer AudioPlayer { get { return audioPlayer; } }

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        playerRend = Player.GetComponent<SpriteRenderer>();
        playerObj = Player.GetComponent<MovingObject>();
        audioPlayer = GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!paused)
        {
            //snap to players position
            transform.position = Player.transform.position;

            if (Input.GetMouseButton(1))
            {
                particleEmission += particlesSent * Time.deltaTime;
                SendParticlesTo(playerObj, Mathf.FloorToInt(particleEmission));
                particleEmission -= Mathf.FloorToInt(particleEmission);

                partRend.sortingLayerID = playerRend.sortingLayerID;
                partRend.sortingOrder = playerRend.sortingOrder - 1;

                part.Play();
            }else if (Input.GetMouseButtonUp(1))
            {
                part.Stop();
            }

            //move the particles toward their target
            MoveParticles();
            
        }
        else
        {
            part.Pause();
        }
    }
}
