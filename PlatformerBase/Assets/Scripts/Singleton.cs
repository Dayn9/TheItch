using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton Framework based off: http://wiki.unity3d.com/index.php/Singleton
/// </summary>
/// <typeparam name="T">Class type of singleton</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;

    public static T Instance
    {
        get
        {
            //check if an instance of the singleton already exists
            if(instance == null)
            {
                //find an instance of singleton if there is one
                instance = (T)FindObjectOfType(typeof(T)); 
                if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    //Error: More than one instance of singleton exists
                    Debug.Log("More than one instance of " + typeof(T) + " Singleton");
                    return instance; //return one of them
                }

                //no existing instances of singleton
                if(instance == null)
                {
                    //create a new instance of singleton
                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<T>();
                    singleton.name = typeof(T).ToString();
                }
                //singleton exists
                else
                {
                    Debug.Log("Singleton exists in " + instance.gameObject.name);
                }
                
            }
            return instance;
        }
    }
}
