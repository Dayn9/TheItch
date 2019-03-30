using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; //projecctile object prefab
    [SerializeField] private int poolSize = 0; //number of projectiles allowed

    private List<ProjectileObject> projPool; //object pool of projectiles
    private int poolIndex = 0;

    private ParticleSystem burstSystem;

    void Awake()
    {
        //create the object pool
        projPool = new List<ProjectileObject>();
        GameObject proj;
        for (int p = 0; p < poolSize; p++)
        {
            proj = Instantiate(projectilePrefab);

            proj.transform.parent = transform;
            proj.name = "Projectile " + p;
            proj.SetActive(false);
            proj.GetComponent<ProjectileObject>().ProjectilePool = this;

            projPool.Add(proj.GetComponent<ProjectileObject>());
        }
        //find the burst
        burstSystem = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Create a Burst of projectile particles
    /// </summary>
    /// <param name="position">location of burst</param>
    public void BurstParticles(Vector3 position)
    {
        //go to location
        ParticleSystem.ShapeModule shapeMod = burstSystem.shape;
        shapeMod.position = position;
        //BURST
        burstSystem.Play();
    }

    /// <summary>
    /// Fires one of the projectiles in the pool
    /// </summary>
    /// <param name="position">starting position of the projectile</param>
    /// <param name="direction">starting move direction of the projectile</param>
    public void FireProjectile(Vector3 position, ProjectileDirection direction)
    {
        //loop through pool until valid object is found
        for (int p = 0; p < poolSize; p++)
        {
            poolIndex = (poolIndex + 1) % poolSize;
            //valid objeect found or last search item (insures fireing)
            if (!projPool[poolIndex].Active || p == poolSize - 1)
            {
                //start the projectile at location
                projPool[poolIndex].Direction = direction;
                projPool[poolIndex].transform.position = position;
                projPool[poolIndex].gameObject.SetActive(true);

                //create a burst at the fire location
                BurstParticles(position);
                break;
            }
        }
    }

}
