using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pause : Global {

    private SpriteRenderer render;
    private Animator[] animators;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.enabled = false;

        animators = FindObjectsOfType<Animator>();
        Debug.Log(animators.Length);
    }

    void Update () {
        //toggle paused when P key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = !paused;
            render.enabled = paused;

            //disable all animators when paused
            foreach(Animator anim in animators)
            {
                anim.enabled = !paused;
            }
        }
	}
}
