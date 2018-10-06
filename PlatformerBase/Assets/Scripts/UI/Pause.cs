using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pause : Global {

    private SpriteRenderer render; //ref to spriteRenderer on this gameobject
    private SpriteRenderer[] childRender; //ref to the spriteRenderers on all the child gameObjects

    protected static Animator[] animators; //ref to all the animators in the scene

    protected static AudioSource[] audios; //ref t all audioSources in the scene
    public static bool mute = false; //true when audio is muted 

    private bool menuPaused; //true when menu is paused
    protected static bool otherPause = false; //true when something else is paused

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

    private void Update()
    {
        //toggle paused when P or Esc key is pressed
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            menuPaused = !menuPaused;
            render.enabled = menuPaused;
            paused = menuPaused || otherPause;

            //disable all animators when paused
            foreach (Animator anim in animators)
            {
                anim.enabled = !paused;
            }

            SetChildRenders(menuPaused);

        }
        paused = menuPaused || otherPause;
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

    /// <summary>
    /// TODO: handles all the pausing and can be called by child objects
    /// </summary>
    public void PauseGame(bool p)
    {
        otherPause = p;
        paused = menuPaused || otherPause;
        //disable all animators when paused
        foreach (Animator anim in animators)
        {
            anim.enabled = !paused;
        }
    }
}
