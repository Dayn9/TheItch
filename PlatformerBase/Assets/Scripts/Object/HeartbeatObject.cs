using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatObject : MonoBehaviour, IHealthObject
{

    [SerializeField] private int maxHealth; //maximum health of the object
    [SerializeField] private int health; //health of the object
    private bool invulnerable = true; //true when player is immune to damage
    private float floatHealth;

    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }
    public bool Invulnerable { get { return invulnerable; } set { invulnerable = value; } }

    public void FullHeal()
    {
        health = maxHealth;
    }

    public void Heal(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }

    public void Heal(float amount)
    {
        floatHealth = Mathf.Clamp(floatHealth + amount, 0, maxHealth);
        health = (int)floatHealth;
    }

    public void Damage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
    }

    public void Damage(float amount)
    {
        floatHealth = Mathf.Clamp(floatHealth - amount, 0, maxHealth);
        health = (int)floatHealth;
    }

    private void Awake()
    {
        //if health is not max, consider it 0
        //health = health == maxHealth ? maxHealth : 0; 
        floatHealth = health;
    }
}