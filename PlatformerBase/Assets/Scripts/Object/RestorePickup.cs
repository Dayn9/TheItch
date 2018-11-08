using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RestorePickup : Global {

    //TODO MAKE GENERAL BASE CLASS FOR PARTICLES THAT HAS SEND TO POINT METHOD

    private const int restoreAmount = 10;

    private BoxCollider2D zone;

    private void Awake()
    {
        zone = GetComponent<BoxCollider2D>();
        zone.isTrigger = true;
    }

    private void Update()
    {
        if (zone.bounds.Contains((Vector2)MainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition))) { Restore(); }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player") { Restore(); }
    }

    private void Restore()
    {
        Player.GetComponent<IPlayer>().Power.RestoreBPM(restoreAmount);
        gameObject.SetActive(false);
    }
}
