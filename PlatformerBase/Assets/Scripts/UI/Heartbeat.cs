using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(UIAnchor))]
[RequireComponent(typeof(HeartbeatAudioPlayer))]
public class Heartbeat : Global {

    private Transform marker; //ref to renderer of healthbar
    private Vector2 markerOffset = new Vector2(3.75f, -1.875f);
    private Animator heartAnimation; //ref to the animator of the heartbeat

    private Digit[] digits; //ref to the digit readouts
    private VirusShaderController virus;

    private static float bpm = -1; //beats per minute (60 = 1 per second)

    private HeartbeatAudioPlayer audioPlayer; //ref to the attached AudioPlayer

    private const float audioThreshhold = 0.5f; //min change rate to hear heartbeat
    private float changeRate = 0; //rate bpm is currently changing
    private float timer = 0; //fade timer
    private const float fadeTime = 3.0f; //time (seconds) it takes for heartbeat audio to completely fade out

    public float ChangeRate { set { changeRate = value; } }

    private const int minHeartrate = 32;
    private const int maxHeartrate = 168;


    public static float BPM
    {
        get { return bpm; }
        set { bpm = value; }
    }

    // Use this for initialization
    void Awake()
    {
        //find the nessicary components in child gameObjects
        marker = transform.GetChild(0);
        heartAnimation = transform.GetChild(1).GetComponent<Animator>();
        audioPlayer = GetComponent<HeartbeatAudioPlayer>();

        digits = transform.GetComponentsInChildren<Digit>();
        virus = GetComponentInChildren<VirusShaderController>();


        //make sure all the components of child objects are available
        Assert.IsNotNull(marker, "indicator SpriteRenderer not found");
        Assert.IsNotNull(heartAnimation, "heartbeat Animator not found");
        Assert.IsTrue(digits.Length == 3, "cannot find the 3 digits");
        Assert.IsNotNull(virus, "Virus controller not found");
    }

    //assign values to the individual components of the healthbeat 
    void Update()
    {
        if (!paused)
        {
            heartAnimation.speed = bpm / 60.0f; //match animation speed to bpm

            audioPlayer.Speed = bpm / 60.0f; //match the audio speed to bpm
            if (changeRate > audioThreshhold)
            {
                timer = 0;
            }
            else
            {
                timer = Mathf.Clamp(timer + Time.deltaTime, 0, fadeTime);
                audioPlayer.Volume -= (timer / fadeTime) * audioPlayer.Volume;
            }


            //player takes damage when heartrate is too high or low
            if (bpm < minHeartrate || bpm > maxHeartrate)
            {
                Player.GetComponent<IHealthObject>().Damage(1);
            }

            SetDigitNum();

            int position = Mathf.CeilToInt((bpm - 4.0f) / 8);
            //position = 0;
            marker.localPosition = markerOffset + Vector2.right * (position * (1.0f / pixelsPerUnit));

            SetHealth();

        }
    }

    /// <summary>
    /// update the digit readouts to match bpm
    /// aligns digit readout to right
    /// </summary>
    private void SetDigitNum()
    {
        //3 digits
        if (bpm >= 100)
        {
            digits[0].SetNumber((int)(bpm / 100));
            digits[1].SetNumber((int)((bpm % 100) / 10));
            digits[2].SetNumber((int)(bpm % 10));
        }
        //2 digits
        else if (bpm >= 10)
        {
            digits[0].SetNumber((int)(bpm / 10));
            digits[1].SetNumber((int)(bpm % 10));
            digits[2].SetNumber(10);
        }
        //1 digit
        else
        {
            digits[0].SetNumber((int)bpm);
            digits[1].SetNumber(10);
            digits[2].SetNumber(10);
        }
    }

    /// <summary>
    /// sets the color of all the digits
    /// </summary>
    /// <param name="color">new Color</param>
    public void SetDigitColor(Color color)
    {
        digits[0].SetColor(color);
        digits[1].SetColor(color);
        digits[2].SetColor(color);
    }

    /// <summary>
    /// Updates the virus shader to match the current health of the player
    /// </summary>
    private void SetHealth()
    {
        if (virus)
        {
            IHealthObject player = Player.GetComponent<IHealthObject>();
            virus.virusMat.SetFloat("_Cutoff", Mathf.Lerp(virus.virusMat.GetFloat("_Cutoff"), 1 - (player.Health / (float)player.MaxHealth), 0.2f));
        }
       
    }

}
