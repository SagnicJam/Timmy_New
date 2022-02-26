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
            PlayerCollider playerCollider = other.gameObject.GetComponent<PlayerCollider>();
            if (playerCollider.playerController != null)
            {
                if (!playerCollider.playerController.isInvulnerable)
                {
                    playerCollider.playerController.TravelToRampLaunchPoint(startPoint, launchPoint);

                    Debug.Log("<color=blue>Ramp </color>");
                }
            }

        }
    }
}
