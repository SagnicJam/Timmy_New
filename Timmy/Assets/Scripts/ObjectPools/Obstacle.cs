using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Obstacle : MonoBehaviour
{
    public List<GameObject> deactivatedGOObstacles;
    public ObstacleTrigger obstacleTrigger;
    IObjectPool<Obstacle> obstaclePool;
    public void SetPool(IObjectPool<Obstacle> pool)
    {
        obstaclePool = pool;
    }

    public void InitialiseObstacle(bool triggerNextObstacle)
    {
        obstacleTrigger.gameObject.SetActive(triggerNextObstacle);
    }

    public void DestroyToPool()
    {
        if (obstaclePool != null)
        {
            //on return object code
            foreach (GameObject item in deactivatedGOObstacles)
            {
                item.SetActive(true);
            }
            deactivatedGOObstacles.Clear();
            obstaclePool.Release(this);
        }
    }

    public void Deactivate(GameObject obstacleGO)
    {
        deactivatedGOObstacles.Add(obstacleGO);
        obstacleGO.SetActive(false);
    }
}
