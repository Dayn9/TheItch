using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pause : Global {

    private SpriteRenderer[] myRenderers; //ref to the spriteRenderers on this and all the child gameObjects

    protected static Animator[] animators; //ref to all the animators in the scene

    protected static AudioSource[] audios; //ref t all audioSources in the scene
    public static bool mute = false; //true when audio is muted 

    private Fade fade; //ref to the fade object in the scene 
    protected static bool menuPaused = false; //true when menu is paused
    protected static bool otherPause = false; //true when something else is paused


    private void Awake()
    {
        fade = transform.parent.GetComponentInChildren<Fade>();

        //find all the required references
        SpriteRenderer[] childRender = GetComponentsInChildren<SpriteRenderer>();
        myRenderers = new SpriteRenderer[childRender.Length + 1];
        for(int r = 0; r< childRender.Length; r++)
        {
            myRenderers[r] = childRender[r];
        }
        myRenderers[myRenderers.Length - 1] = GetComponent<SpriteRenderer>();

        SetRenders(false);

        animators = FindObjectsOfType<Animator>();
        audios = FindObjectsOfType<AudioSource>();

        menuPaused = false; //level should never start paused
        otherPause = false;
    }

    private void Update()
    {
        //toggle paused when P or Esc key is pressed
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            menuPaused = !menuPaused;
            paused = menuPaused || otherPause;

            //disable all animators when paused
            foreach (Animator anim in animators)
            {
                anim.enabled = !paused;
            }

            SetRenders(menuPaused);
            //send the fade object the attached sprite rendereres
            if (menuPaused) {
                
                fade.EnableFade(myRenderers);
            } else {
                fade.DisableFade(myRenderers);
            }
            
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
    private void SetRenders(bool state)
    {
        foreach(SpriteRenderer rend in myRenderers)
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
