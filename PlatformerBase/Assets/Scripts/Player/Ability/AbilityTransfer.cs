using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class AbilityTransfer : BloodParticle {

    [SerializeField] private float heartrateUsed;

    private CircleCollider2D coll; //ref to this objects collider
    //private Animator anim; //ref to this objects animator
    //private SpriteRenderer rend; //ref to this objects sprite renderer

    private SpriteRenderer playerRend; //ref to players sprite renderer

    // Use this for initialization
    protected override void Awake () {

        base.Awake();

        coll = GetComponent<CircleCollider2D>();
        coll.enabled = false;
        //anim = GetComponent<Animator>();
        //rend = GetComponent<SpriteRenderer>();

        playerRend = Player.GetComponent<SpriteRenderer>();
    }

    
	
	// Update is called once per frame
	void Update () {
        if (!paused)
        {
            //snap to players position
            transform.position = Player.transform.position;

            MoveParticles();

            if (Input.GetKeyUp(KeyCode.X) || Input.GetMouseButtonUp(0))
            {
                //try and turn set regular color when key released
                hbPower.SetDamageColor(false);
            }
            if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(0))
            {
                //anim.SetBool("Grow", true);
                //anim.SetBool("Shrink", false);
                coll.enabled = true;
                //part.Play();

                

                //set render properties to be right behind player
                //rend.sortingLayerID = playerRend.sortingLayerID;
                //rend.sortingOrder = playerRend.sortingOrder - 2;
                partRend.sortingLayerID = playerRend.sortingLayerID;
                partRend.sortingOrder = playerRend.sortingOrder -1;

                //set 
                hbPower.RemoveBPM(heartrateUsed * Time.deltaTime);
                hbPower.SetDamageColor(true);
            }
            else
            { 
                //anim.SetBool("Grow", false);
                //anim.SetBool("Shrink", true);
                coll.enabled = false;
                //part.Stop();
            }
        }
        else
        {
            part.Pause();
        }
	}
}
