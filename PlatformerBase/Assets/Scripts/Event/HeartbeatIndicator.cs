using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HeartbeatIndicator : MonoBehaviour {

    private float totalHealth; //how much health is required for the object to be fully healed
    private float currentHealth = 0; //current health of the object

    private SpriteRenderer healthbarFull; //ref to the child object that displays full health
    private SpriteRenderer healthbarEmpty; //ref to the child object the displays empty health

    private SpriteRenderer rend;

    public float Total {
        get { return totalHealth; }
        set {
            totalHealth = value;
            healthbarEmpty.size = new Vector2(totalHealth / 10, healthbarEmpty.size.y);

            healthbarFull.transform.localPosition = new Vector3(-totalHealth / 20, healthbarFull.transform.localPosition.y);
        }
    }
    public float CurrentHealth {
        get { return currentHealth; }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, totalHealth); //set the health of the object
            healthbarFull.size = new Vector2(currentHealth / 10, healthbarFull.size.y); //update the fullHealth width to match
        }
    }

    private void Awake()
    {
        healthbarEmpty = transform.GetChild(0).GetComponent<SpriteRenderer>();
        healthbarFull = transform.GetChild(1).GetComponent<SpriteRenderer>();
        rend = GetComponent<SpriteRenderer>();

        totalHealth = 0;
        CurrentHealth = 0;
    }

    public void SetSprite(Sprite newSprite)
    {
        rend.sprite = newSprite;
    }
}
