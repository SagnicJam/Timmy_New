using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //trigger next zones
        //spawn newzones
        //return previousZones
        if(other.gameObject.tag=="Player")
        {
            TrackSpawnner.instance.SpawnNextTracks();
            TrackSpawnner.instance.RemoveLastTracks();

            gameObject.SetActive(false);
        }
    }
}
