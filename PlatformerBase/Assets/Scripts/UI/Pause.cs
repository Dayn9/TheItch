using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pause : Global {

    private SpriteRenderer render; //ref to spriteRenderer on this gameobject
    private SpriteRenderer[] childRender; //ref to the spriteRenderers on all the child gameObjects

    private Animator[] animators; //ref to all the animators in the scene

    protected static AudioSource[] audios; //ref t all audioSources in the scene
    public static bool mute = false; //true when audio is muted 

    private void Awake()
    {
        //find all the required references
        render = GetComponent<SpriteRenderer>();
        render.enabled = false;

        childRender = GetComponentsInChildren<SpriteRenderer>();
        SetChildRenders(false);

        animators = FindObjectsOfType<Animator>();
        audios = FindObjectsOfType<AudioSource>(); 
    }

    void Update()
    {
        //toggle paused when P or Esc key is pressed
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            render.enabled = paused;

            //disable all animators when paused
            foreach (Animator anim in animators)
            {
                anim.enabled = !paused;
            }
            SetChildRenders(paused);
        }
        //toggle audio when M key is pressed (Quick action, can also be done through pause menu)
        if (Input.GetKeyDown(KeyCode.M))
        {
            mute = !mute;
            foreach (AudioSource source in audios)
            {
                source.mute = mute;
            }
        }
    }

    /// <summary>
    /// set enabled of all the child renderes
    /// </summary>
    /// <param name="state">activation state</param>
    private void SetChildRenders(bool state)
    {
        foreach(SpriteRenderer rend in childRender)
        {
            rend.enabled = state;
        }
    }
}
