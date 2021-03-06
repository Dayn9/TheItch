﻿using UnityEngine;
using System.Collections.Generic;

public class DontDestroy : MonoBehaviour {

    /// <summary>
    /// Keeps the attached object constant across scenes
    /// </summary>

    private static List<string> objectsToKeep; //list of objects in don't destroy on load by name

	// Use this for initialization
	void Awake () {
        //create the list if there is none
        if(objectsToKeep == null)
        {
            objectsToKeep = new List<string>();
        }

        //add object to list if it isn't already
        if (objectsToKeep.Contains(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        objectsToKeep.Add(gameObject.name);
    }
}
