using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public bool isAlive;
    public float projectileSpeed;
    public float destroyAfter = 3f;
    IObjectPool<Projectile> projectilePool;

    IEnumerator cor;

    public void InitialiseProjectile()
    {
        rb.velocity = Vector3.forward * projectileSpeed;
        isAlive = true;
        if (cor == null)
        {
            cor = DestroyCor();
            StartCoroutine(cor);
        }
    }


    public float temp;
    IEnumerator DestroyCor()
    { 
        if(isAlive)
        {
            while (temp < destroyAfter)
            {
                temp += Time.deltaTime;
                yield return null;
            }
            DestroyToPool();
            temp = 0;
            yield break;
        }
    }

    public void SetPool(IObjectPool<Projectile> pool)
    {
        projectilePool = pool;
    }

    public void DestroyToPool()
    {
        if (projectilePool != null)
        {
            //on return object code
            isAlive = false;
            projectilePool.Release(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            ObstacleCollider obstacleCollider = other.gameObject.GetComponent<ObstacleCollider>();
            if(!(obstacleCollider is WaterPuddle))
            {
                obstacleCollider.obstacle.Deactivate(other.gameObject);
                if (cor != null)
                {
                    StopCoroutine(cor);
                    cor = null;
                }
                DestroyToPool();
            }
        }
    }
}
