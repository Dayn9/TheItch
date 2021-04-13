using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointEvent : MonoBehaviour
{
    [SerializeField] private EventTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    [SerializeField] private Checkpoint targetCheckpoint;

    void Start()
    {
        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += SoloCheckpoint;
        }
        else
        {
            evTrig.After += SoloCheckpoint;
        }
    }

    public void SoloCheckpoint()
    {
        Checkpoint[] checkpoints = targetCheckpoint.transform.parent.GetComponentsInChildren<Checkpoint>();
        foreach(Checkpoint c in checkpoints)
        {
            if (c.Equals(targetCheckpoint))
            {
                c.SetCheckpoint();
            }
            else
            {
                c.gameObject.SetActive(false);
            }
        }
    }
}
