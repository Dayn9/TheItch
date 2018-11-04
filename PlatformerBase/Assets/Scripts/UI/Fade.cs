using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Fade : Pause
{ 
    private SpriteRenderer render; //ref to this objects sprite renderer 
    private Dictionary<GameObject, int> origionalLayers; //keeps track of the origional sorting layers of objects that go above the fade

    // Use this for initialization
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        render.enabled = false;

        origionalLayers = new Dictionary<GameObject, int>();
    }

    //blocks pause Update
    private void Update()
    {
        
    }

    /// <summary>
    /// turns the fade on and makes sure necissary objects are above the fade
    /// </summary>
    /// <param name="aboveFade">sprites that should appear above the fade</param>
    public void EnableFade(params Renderer[] aboveFade)
    {
        render.enabled = true; //turn the fade on

        for (int i = 0; i < aboveFade.Length; i++)
        {
            //make sure the gameObject isn't already above the fade 
            if (!origionalLayers.ContainsKey(aboveFade[i].transform.gameObject))
            {
                origionalLayers.Add(aboveFade[i].transform.gameObject, aboveFade[i].sortingLayerID); //save the sorting layer to array
                aboveFade[i].sortingLayerID = render.sortingLayerID; //set to the fade sorting layer
                aboveFade[i].sortingOrder += 1; //make sure always above the fade
            }
        }
        render.sortingOrder = menuPaused ? 10 : 0;
    }

    /// <summary>
    /// returns objects to their origional sorting layer and turns off the fade if necissary 
    /// </summary>
    /// <param name="aboveFade"></param>
    public void DisableFade(params Renderer[] aboveFade)
    {
        //return all objects above fade to their origional sorting layers
        for (int i = 0; i < aboveFade.Length; i++)
        {
            if (origionalLayers.ContainsKey(aboveFade[i].transform.gameObject))
            {
                //return all sprite renderers to their origional sorting layer and order
                aboveFade[i].sortingLayerID = origionalLayers[aboveFade[i].transform.gameObject];
                aboveFade[i].sortingOrder -= 1;

                origionalLayers.Remove(aboveFade[i].transform.gameObject);//remove item from origional layers 
            }
        }
        //only disable the fade when there are no more objects above the fade
        if (origionalLayers.Count == 0)
        {
            render.enabled = false;
        }

        render.sortingOrder = menuPaused ? 10 : 0;
    }
}
