using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItems{
    Dictionary<string, GameObject> Items { get; }//list of items the player currently poseses 

    void AddItem(string name, GameObject obj);

    void RemoveItem(string name);
}
