using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollider : MonoBehaviour
{
    public Obstacle obstacle;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerCollider playerCollider = other.gameObject.GetComponent<PlayerCollider>();
            if(playerCollider.playerController != null)
            {
                if(!playerCollider.playerController.isInvulnerable)
                {
                    GameManager.instance.ShowLooseScreen();
                }
            }
        }
    }
}
