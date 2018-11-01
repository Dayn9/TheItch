using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : Global {

    [SerializeField] private GameObject testRing;
    //private GameObject ring;

    // Use this for initialization
    void Awake () {
        /*ring =*/ Instantiate(testRing);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
