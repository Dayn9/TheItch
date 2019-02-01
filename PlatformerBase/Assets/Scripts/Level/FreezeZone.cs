using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FreezeZone : Global {

    [SerializeField] private bool setFreeze;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.GetComponent<MovingObject>().Frozen = setFreeze;
        }
    }

}
