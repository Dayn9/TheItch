using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioPlayer))]
public class AbilityTransfer : BloodParticle {

    [SerializeField] private float heartrateUsed; //heartrate used when ability transfer is used

    private SpriteRenderer playerRend; //ref to players sprite renderer

    //accessor for audioPlayer (used by Ability Handler)
    public AudioPlayer AudioPlayer { get { return audioPlayer; } }

    // Use this for initialization
    protected override void Awake () {

        base.Awake();
        playerRend = Player.GetComponent<SpriteRenderer>();
        audioPlayer = GetComponent<AudioPlayer>();
    }
    
	// Update is called once per frame
	void Update () {
        if (!paused)
        {
            //snap to players position
            transform.position = Player.transform.position;

            //move the particles toward their target
            MoveParticles(); 

            if (Input.GetMouseButton(0) && AbilityHandler.IsUnlocked(0))
            {
                partRend.sortingLayerID = playerRend.sortingLayerID;
                partRend.sortingOrder = playerRend.sortingOrder -1;
                hbPower.RemoveBPM(heartrateUsed * Time.deltaTime);
            }
        }
        else
        {
            part.Pause();
            //reset the color
        }
	}
}
