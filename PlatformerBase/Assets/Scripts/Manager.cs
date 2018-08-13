using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager> {
    private GameObject mainCamera;

    [SerializeField] public int pixelsPerUnit = 8;

    public GameObject MainCamera
    {
        get
        {
            if(mainCamera == null)
            {
                mainCamera = FindObjectOfType<Camera>().gameObject;
                if (mainCamera == null)
                {
                    Debug.Log("There needs to be a camera in the scene");
                }
            }
            return mainCamera;
        }
    }

    private void Start()
    {
        if(GetComponent<Camera>() != null)
        {
            mainCamera = gameObject;
        }
        mainCamera = FindObjectOfType<Camera>().gameObject;
        if(mainCamera == null)
        {
            Debug.Log("There needs to be a camera in the scene");
        }
    }


}
