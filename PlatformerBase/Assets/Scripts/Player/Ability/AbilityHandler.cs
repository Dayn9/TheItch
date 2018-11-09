using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : Global {

    [SerializeField] private GameObject testRing;
    private ParticleSystem part;

    private AbilityTransfer powerOne;

    public AbilityTransfer PowerOne { get { return powerOne; } }
    //private GameObject ring;

    // Use this for initialization
    void Awake () {
        powerOne = Instantiate(testRing).GetComponent<AbilityTransfer>();
        part = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(0))
        {
            part.Play();
        }
        else
        {

            part.Stop();
        }
    }
}
