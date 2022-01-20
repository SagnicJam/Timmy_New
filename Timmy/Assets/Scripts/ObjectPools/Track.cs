using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Track : MonoBehaviour
{
    IObjectPool<Track> trackPool;

    public TrackTrigger trackTrigger;

    public List<EnvironmentSpawnner> environmentSpawnner;

    public void InitialiseTrack(bool triggerNextTrack)
    {
        trackTrigger.gameObject.SetActive(triggerNextTrack);
    }

    List<GameObject> activeGameObjects = new List<GameObject>();

    public void ActivateEnvironmentObjects()
    {
        //get total
        for (int i = 0; i < environmentSpawnner.Count; i++)
        {
            List<int> weightList = new List<int>();
            for (int j = 0; j < environmentSpawnner[i].environmentDatas.Count; j++)
            {
                weightList.Add(environmentSpawnner[i].environmentDatas[j].weight);
            }
            int index = GetRandomWeightedUtility.GetWeightedIndex(weightList);
            activeGameObjects.Add(environmentSpawnner[i].environmentDatas[index].go);
            environmentSpawnner[i].environmentDatas[index].go.SetActive(true);
        }
    }

    public void SetPool(IObjectPool<Track>pool)
    {
        trackPool = pool;
    }

    public void DestroyToPool()
    {
        if (trackPool != null)
        {
            //on return object code
            trackPool.Release(this);
        }
    }

    public void ClearEnvironmentObjectsAndTrigger()
    {
        trackTrigger.gameObject.SetActive(false);
        foreach (GameObject item in activeGameObjects)
        {
            item.SetActive(false);
        }
        activeGameObjects.Clear();
    }
}
