using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileSpawnner : MonoBehaviour
{
    public static ProjectileSpawnner instance;

    public Projectile projectilePrefab;
    ObjectPool<Projectile> projectilePool;

    void Awake()
    {
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnTakeProjectileFromPool, OnReturnProjectileToPool);
        instance = this;
    }

    Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(projectilePrefab);
        projectile.SetPool(projectilePool);
        return projectile;
    }

    public Projectile SpawnProjectile(Vector3 projectilePosition)
    {
        Projectile projectile = projectilePool.Get();
        projectile.transform.position = projectilePosition;
        projectile.InitialiseProjectile();
        //code for initialise
        return projectile;
    }

    void OnTakeProjectileFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        projectile.InitialiseProjectile();
    }

    void OnReturnProjectileToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }
}
