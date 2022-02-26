using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : ObstacleCollider
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerCollider playerCollider = other.gameObject.GetComponent<PlayerCollider>();
            if (playerCollider.playerController != null)
            {
                if (!playerCollider.playerController.isInvulnerable)
                {
                    playerCollider.playerController.IncreasePlayerMoveSpeed();

                    Debug.Log("<color=blue>Black hole </color>");
                }
            }
        }
    }
}
