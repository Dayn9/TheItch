using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AbilityHandler : Global {

    /// <summary>
    /// in charge of the different abilities the player has
    /// </summary>

    [SerializeField] private GameObject abilityOnePrefab; //prefab of the first bleed ability
    private ParticleSystem part; //particle system attached to object

    private AbilityTransfer powerOne; //ref to the abilityTransfer Compnent of the First ability

    public AbilityTransfer PowerOne { get { return powerOne; } }
    //private GameObject ring;

    // Use this for initialization
    void Awake () {
        powerOne = Instantiate(abilityOnePrefab).GetComponent<AbilityTransfer>();
        part = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        //play the blled particle effect when mouse down
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
