using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : Pause {

    private void Awake()
    {
        fade = transform.parent.GetComponentInChildren<Fade>();

        //find all the required references
        SpriteRenderer[] childRender = GetComponentsInChildren<SpriteRenderer>();
        myRenderers = new SpriteRenderer[childRender.Length + 1];
        for (int r = 0; r < childRender.Length; r++)
        {
            myRenderers[r] = childRender[r];
        }
        myRenderers[myRenderers.Length - 1] = GetComponent<SpriteRenderer>();

        SetRenders(false);
    }

    private void Update()
    {
        //toggle paused when P or Esc key is pressed
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) && menuPaused)
        {
            DisplaySettings(false);
        }
    }

    public void DisplaySettings(bool p)
    {
        SetRenders(p);
        if (p)
        {
            fade.EnableFade(myRenderers);
            menuPaused = true;
        }
        else
        {
            fade.DisableFade(myRenderers);
        }
    }
}
