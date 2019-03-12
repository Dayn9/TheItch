using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireEvent : MonoBehaviour
{
    [SerializeField] private EventTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    [Space(10)]
    [SerializeField] private GameObject projectilePrefab; //projecctile object to fire
    private GameObject myProjectile;
    [SerializeField] private Vector2 projectileVelocity;

    private void Awake()
    {
        //create and set propeties of the projectile
        myProjectile = Instantiate(projectilePrefab);
        myProjectile.GetComponent<MovingObject>().MoveVelocity = projectileVelocity;
        myProjectile.SetActive(false);
    }

    void Start()
    {
        //subscribe to proper event
        if (beforeAfter)
        {
            evTrig.Before += new triggered(FireProjectile);
        }
        else
        {
            evTrig.After += new triggered(FireProjectile);
        }
    }

    /// <summary>
    /// activates the projectile at location
    /// </summary>
    private void FireProjectile()
    {
        myProjectile.transform.position = transform.position;
        myProjectile.SetActive(true);
    }
}

