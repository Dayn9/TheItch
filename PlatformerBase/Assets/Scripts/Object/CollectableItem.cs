using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour {
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!collected && coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collected = true;
            coll.gameObject.GetComponent<IItems>().AddItem(this.gameObject.name, this.gameObject);
            Destroy(this); //remove collectable item script from item
        }
    }
}
