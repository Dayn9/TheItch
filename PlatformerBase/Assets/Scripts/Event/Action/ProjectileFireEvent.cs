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
    [SerializeField] private GameObject projectilePrefab; //projecctile object to fire
    private GameObject myProjectile;
    [SerializeField] private ProjectileDirection direction;

    private void Awake()
    {
        //create and set propeties of the projectile
        myProjectile = Instantiate(projectilePrefab);
        myProjectile.GetComponent<ProjectileObject>().Direction = direction;
        myProjectile.SetActive(false);

        Assert.IsTrue(transform.position.y % 1 == 0.5f, "Projectiles should be fired at an .5 number position");
        Assert.IsTrue(transform.position.x % 1 == 0.5f, "Projectiles should be fired at an .5 number position");
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
        myProjectile.GetComponent<ProjectileObject>().Direction = direction;
    }
}

