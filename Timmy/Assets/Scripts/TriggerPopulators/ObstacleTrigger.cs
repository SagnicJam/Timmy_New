using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        //trigger next zones
        //spawn newzones
        //return previousZones
        if (other.gameObject.tag == "Player")
        {
            if(ObstacleManager.instance.currentplayerObstacleInitialCollisions>= ObstacleManager.instance.playerObstacleInitialCollisions)
            {
                ObstacleManager.instance.SpawnNextObstacles();
                ObstacleManager.instance.RemoveLastObstacles();
            }
            else
            {
                ObstacleManager.instance.currentplayerObstacleInitialCollisions++;
            }
        }
    }
}
