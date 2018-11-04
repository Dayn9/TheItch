using UnityEngine;
using System.Collections.Generic;

public class DontDestroy : MonoBehaviour {

    private static List<string> objectsToKeep;

	// Use this for initialization
	void Awake () {
        if(objectsToKeep == null)
        {
            objectsToKeep = new List<string>();
        }

        if (objectsToKeep.Contains(gameObject.name))
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
