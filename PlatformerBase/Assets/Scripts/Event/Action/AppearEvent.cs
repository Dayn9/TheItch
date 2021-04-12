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
            evTrig.Before += () => SetAllRenderers(appDis) ;
        }
        else
        {
            evTrig.After += () => SetAllRenderers(appDis);
        }
        SetAllRenderers(!appDis);
    }
	
    /// <summary>
    /// sets the activation state of all of the renderers and colliders 
    /// </summary>
    /// <param name="active">enable / disabled</param>
    private void SetAllRenderers(bool active)
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>(); 
        foreach(Renderer rend in rends)
        {
            rend.enabled = active;
        }
        /*
        Collider2D[] colls = GetComponentsInChildren<Collider2D>();
        foreach(Collider2D coll in colls)
        {
            coll.enabled = active;
        }
        */
    }
}
