using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(UIAnchor))]
public class Heartbeat : Global {

    private TextMesh bpmReadout;//ref to Text in bpm readout (3-digit number)
    private SpriteRenderer healthbar; //ref to renderer of healthbar
    private Animator heartAnimation; //ref to the animator of the heartbeat

    private float numHealthbarTicks; //pixel width of the healthbar
    private int bpm = 60; //beats per minute (default 60 or 1 per second)

    public int BPM {
        get { return bpm; }
        set { bpm = value > 0 && value < 250 ? value : bpm; } //bpm must be between 0 - 250
    }

    // Use this for initialization
    void Awake () {
        //find the nessicary components in child gameObjects
        bpmReadout = transform.GetChild(0).GetComponent<TextMesh>();
        healthbar = transform.GetChild(1).GetComponent<SpriteRenderer>();
        heartAnimation = transform.GetChild(2).GetComponent<Animator>();

        //make sure all the components of child objects are available
        Assert.IsNotNull(bpmReadout, "bpmReadout TextMesh not found");
        Assert.IsNotNull(healthbar, "healthbar SpriteRenderer not found");
        Assert.IsNotNull(heartAnimation, "heartbeat Animator not found");

        //find the number of pixels in the healthbar
        numHealthbarTicks = (int)(healthbar.size.x * pixelsPerUnit);
    }
	
    //assign values to the individual components of the healthbeat 
	void Update () {
        SetHealth(Player.GetComponent<IHealthObject>().Health, Player.GetComponent<IHealthObject>().MaxHealth); //update health
        heartAnimation.speed = bpm / 60.0f; //match animation speed to bpm
        bpmReadout.text = bpm.ToString(); //display bpm
	}

    /// <summary>
    /// determines how many pixels to display in the healthbar
    /// </summary>
    /// <param name="currentHealth">current number of health points</param>
    /// <param name="maxHealth">maximum number of heath points</param>
    private void SetHealth(int currentHealth, int maxHealth)
    {
        float width = (currentHealth / (float)maxHealth) * (numHealthbarTicks / (float)pixelsPerUnit); //perfectly scaled width
        healthbar.size = new Vector2(width - width % (1.0f / pixelsPerUnit), healthbar.size.y); //set healthbar size and make sure pixel size 
    }
}
