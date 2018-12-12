using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class AbilityUnlock : BloodParticle {

    [SerializeField] private int numParticles;
    [SerializeField] private int abilityUnlocked;

    private SpriteRenderer render;

    protected override void Awake()
    {
        base.Awake();
        render = GetComponent<SpriteRenderer>();

        audioPlayer = GetComponentInParent<AudioPlayer>();
    }

    private void Update()
    {
        MoveParticles();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player.GetComponent<AbilityHandler>().Unlock(abilityUnlocked);
            render.enabled = false;
            SendParticlesTo(Player.GetComponent<MovingObject>(), numParticles);

            part.Play();
        }
    }
}
