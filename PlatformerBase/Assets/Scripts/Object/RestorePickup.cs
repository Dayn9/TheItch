using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class RestorePickup : BloodParticle {

    private const int healAmount = 1;
    private const int restoreAmount = 20;

    private BoxCollider2D zone;
    private SpriteRenderer render;

    protected override void Awake()
    {
        base.Awake();

        zone = GetComponent<BoxCollider2D>();
        zone.isTrigger = true;

        render = GetComponent<SpriteRenderer>();
        render.enabled = true;

        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    private void Update()
    {
        MoveParticles();
        
        if (!sending && zone.bounds.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)))
        {
            Restore();
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(!sending && coll.tag == "Player") { Restore(); }
    }

    private void Restore()
    {
        //transform.parent = Player.transform;
        SendParticlesTo(Player.GetComponent<MovingObject>(), restoreAmount * 10);

        Player.GetComponent<IHealthObject>().Heal(healAmount);
        Player.GetComponent<IPlayer>().Power.RestoreBPM(restoreAmount);
        render.enabled = false;
        zone.enabled = false;

        part.Play();
    }
}
