﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class AbilityUnlock : BloodParticle, ILevelData {

    [SerializeField] private int numParticles;
    [SerializeField] private int abilityUnlocked;
    [SerializeField] private UnlockText text;

    private SpriteRenderer render;
    private Collider2D coll;

    public bool State { get { return AbilityHandler.IsUnlocked(abilityUnlocked); } }
    public string Name { get { return gameObject.name; } }

    protected override void Awake()
    {
        base.Awake();
        render = GetComponent<SpriteRenderer>();
        audioPlayer = GetComponentInParent<AudioPlayer>();
        coll = GetComponent<Collider2D>();

        //Assert.IsNotNull(text, "Need to add Unlock text for " + gameObject.name);
    }

    public void OnLevelLoad(bool state)
    {
        if (AbilityHandler.IsUnlocked(abilityUnlocked))
        {
            render = GetComponent<SpriteRenderer>();
            coll = GetComponent<Collider2D>();
            render.enabled = false;
            coll.enabled = false;
        }
    }

    private void Update()
    {
        MoveParticles();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //audioPlayer.PlaySound(0);
            Player.GetComponent<AbilityHandler>().Unlock(abilityUnlocked);
            render.enabled = false;
            SendParticlesTo(Player.GetComponent<MovingObject>(), numParticles);
            if(text != null)
            {
                text.ShowText();
            }
            coll.enabled = false;
            part.Play();
        }
    }


}
