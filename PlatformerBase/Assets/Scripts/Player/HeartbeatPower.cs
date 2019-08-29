using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Heartbeat))]
public class HeartbeatPower : Global {

    [SerializeField] private float initialBPM; //initial value for the BPM
    [SerializeField] private float maxHeartRate; //maximum value for bpm
    [SerializeField] private float deltaHeartRate; //rate of change when bpm is being updated 

    private float targetBPM; //value bpm is animating towards

    private Heartbeat heartbeat; //ref to the attched Heartbeat controller

    [SerializeField] private Color bpmReadoutNormal; //base color of BPM readout
    [SerializeField] private Color bpmReadoutDamage; //color of BPM readoutr when damaged
    [SerializeField] private Color bpmReadoutHeal; //color of BPM raeadout when healing

    [Space(10)]
    [SerializeField] private Color outlineDefault;
    [SerializeField] private Color outlineTransfer;
    [SerializeField] private Color outlineAbsorb;
    [SerializeField] private Color outlineWater;
    [SerializeField] private Color outlineVirus;

    private SpriteRenderer render;
    private Color[] outlineColorLookup;

    private Color targetOutlineColor;
    private Color targetDigitColor;

    public Heartbeat Heartbeat
    {
        get
        {
            if (heartbeat == null) { heartbeat = GetComponent<Heartbeat>(); }
            return heartbeat;
        }
    }

    void Awake()
    {
        heartbeat = GetComponent<Heartbeat>();
        if(Heartbeat.BPM == -1)
        {
            Heartbeat.BPM = initialBPM;
        }
        targetBPM = Heartbeat.BPM;

        render = GetComponent<SpriteRenderer>();
        outlineColorLookup = new Color[] { outlineDefault, outlineTransfer, outlineAbsorb, outlineWater, outlineVirus };
        SetOutlineColor(0);
    }

    private void Start()
    {
        heartbeat.SetDigitColor(bpmReadoutNormal);
    }

    /// <summary>
    /// { outlineDefault, outlineTransfer, outlineAbsorb, outlineWater, outlineVirus }
    /// </summary>
    /// <param name="colorKey">index from color lookup</param>
    public void SetOutlineColor(int colorKey)
    {
        if(colorKey < 0 || colorKey > outlineColorLookup.Length) { colorKey = 0; } //out of range goes to zero
        if (colorKey == 0)
        {
            targetOutlineColor = outlineDefault;
            targetDigitColor = bpmReadoutNormal;
        }
        else
        {
            targetOutlineColor = outlineColorLookup[colorKey];
            targetDigitColor = outlineColorLookup[colorKey];
        }
        
    }
    
    /// <summary>
    /// increase target BPM
    /// </summary>
    /// <param name="restore">amount to add</param>
    public void RestoreBPM(float restore = 1)
    {
        targetBPM = targetBPM + restore;
        targetBPM = Mathf.Clamp(targetBPM, 0, maxHeartRate);
    }

    /// <summary>
    /// decrease target BPM
    /// </summary>
    /// <param name="remove">amount to take away</param>
    public void RemoveBPM(float remove = 1)
    {
        targetBPM = targetBPM - remove;
        targetBPM = Mathf.Clamp(targetBPM, 0, maxHeartRate);
    }

    void Update()
    {
        if (!paused)
        {
            //lerp color to target
            render.material.SetColor("_Outline", Color.Lerp(render.material.GetColor("_Outline"), targetOutlineColor, 0.1f));
            heartbeat.SetDigitColor(Color.Lerp(heartbeat.GetDigitColor(), targetDigitColor, 0.1f));
            Heartbeat.BPM = targetBPM;

            /*
            if (targetBPM != Heartbeat.BPM)
            {
                float difference = targetBPM - Heartbeat.BPM;
                heartbeat.ChangeRate = Mathf.Abs(difference);
                //snap to heartrate when moveDistance is small enough
                if (difference * Mathf.Sign(difference) < deltaHeartRate * Time.deltaTime)
                {
                    Heartbeat.BPM = targetBPM;
                    heartbeat.SetDigitColor(bpmReadoutNormal);
                }
                else
                {
                    Heartbeat.BPM += Mathf.Sign(difference) * deltaHeartRate * Time.deltaTime;
                    heartbeat.SetDigitColor(Mathf.Sign(difference) > 0 ? bpmReadoutHeal : bpmReadoutDamage);
                }
            } */
        }
    }
}
