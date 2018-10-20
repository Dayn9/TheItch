using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class Highlight : Global {

    [SerializeField] protected Sprite active; //sprite to display when player touching
    [SerializeField] protected Sprite inactive; //sprite to display when player not touvhing
    private SpriteRenderer render; //ref to object's sprite renderer

    protected virtual void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        render.sprite = inactive;
    }

    //highlight when player is touching
    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            render.sprite = active;
        }
    }

    //remove highlight when player is no longer touching
    protected virtual void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            render.sprite = inactive;
        }  
    }
}
