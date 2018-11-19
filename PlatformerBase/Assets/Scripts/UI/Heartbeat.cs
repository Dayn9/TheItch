using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(UIAnchor))]
public class Heartbeat : Global {

    private SpriteRenderer healthbar; //ref to renderer of healthbar
    private Animator heartAnimation; //ref to the animator of the heartbeat

    private Digit[] digits;

    private float numHealthbarTicks; //pixel width of the healthbar
    private static float bpm = -1; //beats per minute (60 = 1 per second)

    public float BPM
    {
        get { return bpm; }
        set { bpm = value; }
    }


    // Use this for initialization
    void Awake () {
        //find the nessicary components in child gameObjects
        healthbar = transform.GetChild(0).GetComponent<SpriteRenderer>();
        heartAnimation = transform.GetChild(1).GetComponent<Animator>();

        digits = transform.GetComponentsInChildren<Digit>();

        //make sure all the components of child objects are available
        Assert.IsNotNull(healthbar, "healthbar SpriteRenderer not found");
        Assert.IsNotNull(heartAnimation, "heartbeat Animator not found");
        Assert.IsTrue(digits.Length == 3, "cannot find the 3 digits");

        //find the number of pixels in the healthbar
        numHealthbarTicks = (int)(healthbar.size.x * pixelsPerUnit);
    }

    //assign values to the individual components of the healthbeat 
    void Update()
    {
        if (!paused)
        {
            SetHealth(Player.GetComponent<IHealthObject>().Health, Player.GetComponent<IHealthObject>().MaxHealth); //update health

            heartAnimation.speed = bpm / 60.0f; //match animation speed to bpm

            if(bpm < 1)
            {
                Player.GetComponent<IHealthObject>().TakeDamage(1);
            }

            SetDigitNum();
        }
	}

    /// <summary>
    /// update the digit readouts to match bpm
    /// </summary>
    private void SetDigitNum()
    {
        if(bpm >= 100)
        {
            digits[0].SetNumber((int)(bpm / 100));
            digits[1].SetNumber((int)((bpm % 100) / 10));
            digits[2].SetNumber((int)(bpm % 10));
        }
        else
        {
            //aligns digit readout to right
            digits[0].SetNumber((int)(bpm / 10));
            digits[1].SetNumber((int)(bpm % 10));
            digits[2].SetNumber(10);
        }
    }

    public void SetDigitColor(Color color)
    {
        digits[0].SetColor(color);
        digits[1].SetColor(color);
        digits[2].SetColor(color);
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
