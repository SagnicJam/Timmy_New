using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleSpawnner : MonoBehaviour
{
    public int weight;
    public int length;
    public Obstacle obstaclePrefab;
    ObjectPool<Obstacle> obstaclePool;


    void Awake()
    {
        obstaclePool = new ObjectPool<Obstacle>(CreateObstacle, OnTakeObstacleFromPool, OnReturnObstacleToPool);
    }

    Obstacle CreateObstacle()
    {
        Obstacle obstacle = Instantiate(obstaclePrefab);
        obstacle.SetPool(obstaclePool);
        return obstacle;
    }

    public Obstacle SpawnObstacle(Vector3 obstaclePosition)
    {
        Obstacle obstacle = obstaclePool.Get();
        obstacle.transform.position = obstaclePosition;
        //code for initialise
        return obstacle;
    }

    void OnTakeObstacleFromPool(Obstacle obstacle)
    {
        obstacle.gameObject.SetActive(true);
    }

    void OnReturnObstacleToPool(Obstacle obstacle)
    {
        obstacle.gameObject.SetActive(false);
    }
}
