using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager instance;

    public ObstacleSpawnner[] obstacleSpawnners;

    [Header("ObstacleData")]
    public int freeTracks;
    public float trackLength;
    public float cellZLength;
    public int spawnFrontCount;
    public int initialSpawnCount;
    public int spawnTriggerAt;
    Vector3 nextSpawnPoint;
    int obstacleIndexSpawnnedLast = 0;
    public int playerObstacleInitialCollisions;
    public int currentplayerObstacleInitialCollisions;
    int[] weights;
    Queue<Obstacle> obstacleQueue = new Queue<Obstacle>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        weights = new int[obstacleSpawnners.Length];
        nextSpawnPoint = Vector3.forward * trackLength * freeTracks;
        for (int i = 0; i < obstacleSpawnners.Length; i++)
        {
            weights[i] = obstacleSpawnners[i].weight;
        }
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnNextObstacle();
        }
    }

    void SpawnNextObstacle()
    {
        ObstacleSpawnner obstacleSpawnner = obstacleSpawnners[GetRandomWeightedUtility.GetWeightedIndex(weights)];
        Obstacle obstacle=obstacleSpawnner.SpawnObstacle(nextSpawnPoint);
        nextSpawnPoint += Vector3.forward * obstacleSpawnner.length* cellZLength;
        obstacleQueue.Enqueue(obstacle);
        obstacleIndexSpawnnedLast++;

        obstacle.InitialiseObstacle((obstacleIndexSpawnnedLast % spawnTriggerAt) == 0);

        //code for initialise
    }

    public void SpawnNextObstacles()
    {
        for (int i = 0; i < spawnFrontCount; i++)
        {
            SpawnNextObstacle();
        }
    }

    public void RemoveLastObstacles()
    {
        for (int i = 0; i < spawnFrontCount; i++)
        {
            RemoveLastObstacle();
        }
    }

    void RemoveLastObstacle()
    {
        Obstacle obstacle = obstacleQueue.Dequeue();
        obstacle.DestroyToPool();
    }
}
