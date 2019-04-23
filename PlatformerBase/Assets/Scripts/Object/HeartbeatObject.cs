using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatObject : MonoBehaviour, IHealthObject
{

    [SerializeField] private int maxHealth; //maximum health of the object
    [SerializeField] private float health; //health of the object
    private bool invulnerable = true; //true when player is immune to damage

    public float Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }
    public bool Invulnerable { get { return invulnerable; } set { invulnerable = value; } }

    public void FullHeal()
    {
        health = maxHealth;
    }

    public void Heal(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void Damage(float amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
    }
}