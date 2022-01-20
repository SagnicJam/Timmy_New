using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class dummyspawnner : MonoBehaviour
{
    ObjectPool<Dummy> trackPool;
    public Dummy prefab;

    private void Awake()
    {
        trackPool = new ObjectPool<Dummy>(CreateLane, OnTakeTrackFromPool, OnReturnTrackToPool);
    }

    public Dummy Spawn(Vector3 position)
    {
        Dummy track = trackPool.Get();
        track.transform.position = position;
        return track;
        //code for initialise
    }

    Dummy CreateLane()
    {
        Dummy lane = Instantiate(prefab);
        lane.SetPool(trackPool);
        return lane;
    }

    void OnTakeTrackFromPool(Dummy track)
    {
        track.gameObject.SetActive(true);
    }

    void OnReturnTrackToPool(Dummy track)
    {
        track.gameObject.SetActive(false);
    }
}
