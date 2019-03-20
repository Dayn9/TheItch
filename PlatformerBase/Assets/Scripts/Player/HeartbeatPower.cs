﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(HeartbeatTest))]
public class HeartbeatPower : Global {

    [SerializeField] private float initialBPM; //initial value for the BPM
    [SerializeField] private float maxHeartRate; //maximum value for bpm
    [SerializeField] private float deltaHeartRate; //rate of change when bpm is being updated 

    private float targetBPM; //value bpm is animating towards

    private HeartbeatTest heartbeat; //ref to the attched Heartbeat controller

    [SerializeField] private Color bpmReadoutNormal; //base color of BPM readout
    [SerializeField] private Color bpmReadoutDamage; //color of BPM readoutr when damaged
    [SerializeField] private Color bpmReadoutHeal; //color of BPM raeadout when healing

    public HeartbeatTest Heartbeat { get {
            if(heartbeat == null) { heartbeat = GetComponent<HeartbeatTest>(); }
            return heartbeat;
        }
    }

    void Awake()
    {
        if(Heartbeat.BPM == -1)
        {
            heartbeat.BPM = initialBPM;
        }
        targetBPM = heartbeat.BPM;
    }

    private void Start()
    {
        heartbeat.SetDigitColor(bpmReadoutNormal);
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
        if (!paused && targetBPM != heartbeat.BPM)
        {
            float difference = targetBPM - heartbeat.BPM;
            heartbeat.ChangeRate = Mathf.Abs(difference);
            //snap to heartrate when moveDistance is small enough
            if (difference * Mathf.Sign(difference) < deltaHeartRate * Time.deltaTime)
            {
                heartbeat.BPM = targetBPM;
                heartbeat.SetDigitColor(bpmReadoutNormal);
            }
            else
            {
                heartbeat.BPM += Mathf.Sign(difference) * deltaHeartRate * Time.deltaTime;
                heartbeat.SetDigitColor(Mathf.Sign(difference) > 0 ? bpmReadoutHeal : bpmReadoutDamage);
            }

        }
    }
}
