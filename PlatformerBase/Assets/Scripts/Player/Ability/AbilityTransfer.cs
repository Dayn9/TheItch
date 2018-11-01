using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class AbilityTransfer : Global { 
    
    private CircleCollider2D coll;
    private Animator anim;
    private SpriteRenderer rend;

    private ParticleSystem part;
    private ParticleSystemRenderer partRend;

    private SpriteRenderer playerRend;

    // Use this for initialization
    void Awake () {
        coll = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();

        part = GetComponent<ParticleSystem>();
        partRend = GetComponent<ParticleSystemRenderer>();

        playerRend = Player.GetComponent<SpriteRenderer>();

        coll.enabled = false;
    }

    //TODO
    public void SendParticlesTo(Vector2 target)
    {
        //send a burst of particles to a specicific position in world space
    }
	
	// Update is called once per frame
	void Update () {
        if (!paused)
        {
            if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(0))
            {
                anim.SetBool("Grow", true);
                anim.SetBool("Shrink", false);
                coll.enabled = true;
                part.Play();

                //set properties to match player values
                transform.position = Player.transform.position;
                rend.sortingLayerID = playerRend.sortingLayerID;
                rend.sortingOrder = playerRend.sortingOrder - 2;
               
                partRend.sortingLayerID = rend.sortingLayerID;
                partRend.sortingOrder = rend.sortingOrder + 1;
            }
            else
            {
                anim.SetBool("Grow", false);
                anim.SetBool("Shrink", true);
                coll.enabled = false;
                part.Stop();
            }
        }
        else
        {
            part.Pause();
        }
	}
}
