using UnityEngine;

public class CollectableItem : Inventory {
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!collected && coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collected = true;
            AddItem(gameObject.name, gameObject);
            Destroy(this); //remove collectable item script from item
        }
    }
}
