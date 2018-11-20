using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioPlayer))]
public class AbilityTransfer : BloodParticle {

    [SerializeField] private float heartrateUsed; //heartrate used when ability transfer is used

    private SpriteRenderer playerRend; //ref to players sprite renderer



    // Use this for initialization
    protected override void Awake () {

        base.Awake();
        playerRend = Player.GetComponent<SpriteRenderer>();

        audioPlayer = GetComponent<AudioPlayer>();
        Assert.IsNotNull(audioPlayer);
    }
    
	// Update is called once per frame
	void Update () {
        if (!paused)
        {
            //snap to players position
            transform.position = Player.transform.position;

            //move the particles toward their target
            MoveParticles(); 

            if (Input.GetKeyUp(KeyCode.X) || Input.GetMouseButtonUp(0))
            {
                //try and turn set regular color when key released
                hbPower.SetDamageColor(false);
            }
            if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(0))
            {
                partRend.sortingLayerID = playerRend.sortingLayerID;
                partRend.sortingOrder = playerRend.sortingOrder -1;
                hbPower.RemoveBPM(heartrateUsed * Time.deltaTime);
                hbPower.SetDamageColor(true);
            }
        }
        else
        {
            part.Pause();
            //reset the color
            hbPower.SetDamageColor(false);
        }
	}
}
