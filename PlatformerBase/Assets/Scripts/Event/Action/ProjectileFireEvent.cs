using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum ProjectileDirection
{
    left, right, up, down
}

public class ProjectileFireEvent : MonoBehaviour
{
    [SerializeField] private EventTrigger evTrig; //eventTrigger 
    [Header("Before/True  -  After/False")]
    [SerializeField] private bool beforeAfter; //when (before/after questCompleted) the event is triggered

    [Space(10)]
    [SerializeField] private ProjectileDirection direction;
    private ProjectilePool projectilePool;

    private void Start()
    {
        projectilePool = transform.parent.GetComponent<ProjectilePool>();
        Assert.IsNotNull(projectilePool, "Projectile Pool not found");

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
        projectilePool.FireProjectile(transform.position, direction);
    }
}

