﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class AbilityUnlock : BloodParticle {

    [SerializeField] private int numParticles;
    [SerializeField] private int abilityUnlocked;
    [SerializeField] private UnlockText text;

    private SpriteRenderer render;
    private Collider2D coll;

    protected override void Awake()
    {
        base.Awake();
        render = GetComponent<SpriteRenderer>();
        audioPlayer = GetComponentInParent<AudioPlayer>();
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {
        MoveParticles();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioPlayer.PlaySound(0);
            Player.GetComponent<AbilityHandler>().Unlock(abilityUnlocked);
            render.enabled = false;
            SendParticlesTo(Player.GetComponent<MovingObject>(), numParticles);
            text.ShowText();
            coll.enabled = false;
            part.Play();
        }
    }
}