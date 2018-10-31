using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTransfer : MonoBehaviour {

    [SerializeField] private GameObject testRing;
    private GameObject ring; 

	// Use this for initialization
	void Awake () {
        ring = Instantiate(testRing, transform);
        ring.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0))
        {
            ring.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.X) || Input.GetMouseButtonUp(0))
        {
            ring.SetActive(false);
        }
	}
}
