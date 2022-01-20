using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class TrackSpawnner : MonoBehaviour
{
    public static TrackSpawnner instance;

    [Header("TrackData")]
    public int spawnFrontCount;
    public int initialSpawnCount;
    public int spawnTriggerAt;
    public float trackSpawnOffset;
    public Track trackPrefab;

    ObjectPool<Track>  trackPool;

    Queue<Track> trackQueue=new Queue<Track>();

    Vector3 latestTrackPosition=Vector3.zero;
    int trackIndexSpawnnedLast = 0;

    private void Awake()
    {
        trackPool = new ObjectPool<Track>(CreateLane,OnTakeTrackFromPool, OnReturnTrackToPool);
        instance = this;
    }

    private void Start()
    {
        SpawnInitialser();
    }

    public void SpawnInitialser()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnNextTrack();
        }
    }

    void SpawnNextTrack()
    {
        Track track = trackPool.Get();
        track.transform.position = latestTrackPosition;
        latestTrackPosition += Vector3.forward * trackSpawnOffset;
        trackIndexSpawnnedLast++;
        trackQueue.Enqueue(track);
        //code for initialise
    }

    public void SpawnNextTracks()
    {
        for (int i = 0; i < spawnFrontCount; i++)
        {
            SpawnNextTrack();
        }
    }

    public void RemoveLastTracks()
    {
        for (int i = 0; i < spawnFrontCount; i++)
        {
            RemoveLastTrack();
        }
    }

    void RemoveLastTrack()
    {
        Track track = trackQueue.Dequeue();
        track.DestroyToPool();
    }

    Track CreateLane()
    {
        Track lane = Instantiate(trackPrefab);
        lane.SetPool(trackPool);
        return lane;
    }

    void OnTakeTrackFromPool(Track track)
    {
        track.gameObject.SetActive(true);
        track.InitialiseTrack((trackIndexSpawnnedLast % spawnTriggerAt) == 0);
        track.ActivateEnvironmentObjects();
    }

    void OnReturnTrackToPool(Track track)
    {
        track.ClearEnvironmentObjectsAndTrigger();
        track.gameObject.SetActive(false);
    }
}

