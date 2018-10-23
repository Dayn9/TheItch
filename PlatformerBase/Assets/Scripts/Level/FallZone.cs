using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour {

    [SerializeField] private GameObject[] fallSections;
    [SerializeField] private Rect zone; // zone should be greater than camera zone

	// Use this for initialization
	void Start () {
		for(int i = 0; i< fallSections.Length; i++)
        {
            fallSections[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
