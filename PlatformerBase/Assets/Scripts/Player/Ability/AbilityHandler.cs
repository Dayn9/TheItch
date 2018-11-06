using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : Global {

    [SerializeField] private GameObject testRing;

    private AbilityTransfer powerOne;

    public AbilityTransfer PowerOne { get { return powerOne; } }
    //private GameObject ring;

    // Use this for initialization
    void Awake () {
        powerOne = Instantiate(testRing).GetComponent<AbilityTransfer>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
