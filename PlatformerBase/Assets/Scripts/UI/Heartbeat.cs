using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(UIAnchor))]
public class Heartbeat : Global {

    private TextMesh bpmReadout;

	// Use this for initialization
	void Awake () {
        bpmReadout = transform.GetChild(0).GetComponent<TextMesh>(); //get the textmesh from the bpmReadout child object
        Assert.IsNotNull(bpmReadout, "bpmReadout TextMesh not found"); //make sure TextMesh is found
	}
	
	// Update is called once per frame
	void Update () {
	}
}
