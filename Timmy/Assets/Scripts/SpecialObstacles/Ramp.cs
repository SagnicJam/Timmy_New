using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : ObstacleCollider
{
    public Transform launchPoint;
    public Transform startPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerCollider>().playerController;
            playerController.TravelToRampLaunchPoint(startPoint,launchPoint);
        }
    }
}
