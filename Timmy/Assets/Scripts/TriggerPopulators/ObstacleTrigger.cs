using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                if(GameManager.instance.CheckLevelEnd())
                {
                    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                    if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
                    {
                        SceneManager.LoadScene(nextSceneIndex);
                        Debug.Log("Loading next scne");
                    }
                    else
                    {
                        Debug.Log("GameCleareed");
                        GameManager.instance.ShowWinScreen();
                    }
                }
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
