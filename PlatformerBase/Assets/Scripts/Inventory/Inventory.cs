using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// add on to Global for classes that interact with inventory 
/// </summary>
public class Inventory : Global {

    private static Dictionary<string, GameObject> items; //player inventory

    public Dictionary<string, GameObject> Items {
        get {
            if(items == null)
            {
                items = new Dictionary<string, GameObject>();
            }
            return items;
        }
    } 
    
    /// <summary>
    /// Add an item into the player's inventory
    /// </summary>
    /// <param name="name">name of the object</param>
    /// <param name="obj">gameobject</param>
    public void AddItem(string name, GameObject obj)
    {
        Items.Add(name, obj);
        obj.AddComponent<UIAnchor>(); //anchor the item in UI
        obj.transform.parent = MainCamera.transform; //make child of the mainCamera
        obj.GetComponent<UIAnchor>().Set(Anchor.topRight, new Vector2(-1.0f, items.Count * -1.125f), true); //TODO: variable pixel offset
    }

    /// <summary>
    /// remove an item from the player's inventory
    /// </summary>
    /// <param name="name"></param>
    public void RemoveItem(string name)
    {
        if (Items.ContainsKey(name))
        {
            Items.Remove(name);
        }
    }
}
