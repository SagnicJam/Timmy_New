using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Obstacle obstacle =GetComponentInParent<Obstacle>();
            obstacle.Deactivate(gameObject);
            JukeBoxUnit.instance.PlayVFX(1,transform.position);
            GameManager.instance.OnCoinCollected();

        }
    }
}
