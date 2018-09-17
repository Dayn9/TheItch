using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearEvent : MonoBehaviour {
    [SerializeField] private EventTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")] 
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered
    [Header("Appear/True  -  Disappear/False")]
    [SerializeField] private bool appDis; //make the object appear/disappear when even is triggered

    void Start () {
        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += new triggered(Appear);
        }
        else
        {
            evTrig.After += new triggered(Appear);
        }
        gameObject.SetActive(!appDis);
	}
	
    /// <summary>
    /// called by event, sets acivation state
    /// </summary>
    private void Appear()
    {
        gameObject.SetActive(appDis);
    }
}
