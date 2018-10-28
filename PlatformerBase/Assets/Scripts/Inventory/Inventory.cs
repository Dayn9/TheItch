using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// add on to Global for classes that interact with inventory 
/// </summary>
public class Inventory : Global {

    private static Dictionary<string, GameObject> items; //player inventory
    protected static Transform inventoryUI;

    //Offsets and spacings between items in inventory
    private const float offsetY = 3.75f; //offset from anchor along y axis
    private const float spacing = -1.5f; //spacing between individual items

    protected static GameObject collectEffect; //instantiated collect Effect

    public Dictionary<string, GameObject> Items {
        get {
            if(items == null)
            {
                items = new Dictionary<string, GameObject>();
            }
            return items;
        }
        //To set items use AddItem() and RemoveItem
    } 
    
    /// <summary>
    /// Add an item into the player's inventory
    /// </summary>
    /// <param name="name">name of the object</param>
    /// <param name="obj">gameobject</param>
    public void AddItem(string name, GameObject obj)
    {
        Items.Add(name, obj);

        obj.transform.parent = inventoryUI; //make child of the inventoryUI
        DisplayItems();
    }

    /// <summary>
    /// remove an item from the player's inventory
    /// </summary>
    /// <param name="name"></param>
    public void RemoveItem(string name)
    {
        if (Items.ContainsKey(name))
        {
            Items[name].GetComponent<CollectableItem>().Eaten(transform);
            //Destroy(Items[name]); //destroy the gameObject
            Items.Remove(name);
        }
        DisplayItems();
    }

    /// <summary>
    /// make the items appear in the correct location on screen
    /// </summary>
    protected void DisplayItems()
    {
        int index = 0;
        foreach(string item in Items.Keys)
        {
            //position item relative to inventoryUI
            Items[item].transform.localPosition = new Vector2(0.0f, offsetY + (index * spacing));
            index++;
        }
    }
}
 