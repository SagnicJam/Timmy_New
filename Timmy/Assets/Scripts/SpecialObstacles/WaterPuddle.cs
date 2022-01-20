using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPuddle : ObstacleCollider
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerCollider>().playerController;
            playerController.SlipPlayer();
        }
    }
}
