using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Singleton<Manager> {

    private GameObject mainCamera;
    public int pixelsPerUnit = 8;
    private Dictionary<string, GameObject> items = new Dictionary<string, GameObject>(); //player inventory

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
    public Dictionary<string, GameObject> Items { get { return items; } } //items is read only (use AddItem and RemoveItem set)

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
    
    /// <summary>
    /// Add an item into the player's inventory
    /// </summary>
    /// <param name="name">name of the object</param>
    /// <param name="obj">gameobject</param>
    public void AddItem(string name, GameObject obj)
    {
        items.Add(name, obj);
        obj.AddComponent<UIAnchor>(); //anchor the item in UI
        obj.transform.parent = MainCamera.transform; //make child of the mainCamera
        obj.GetComponent<UIAnchor>().Set(Anchor.topRight, mainCamera, new Vector2(-1.0f, items.Count * -1.125f), true, 8);
    }

    /// <summary>
    /// remove an item from the player's inventory
    /// </summary>
    /// <param name="name"></param>
    public void RemoveItem(string name)
    {
        if (items.ContainsKey(name))
        {
            items.Remove(name);
        }
    }
}
