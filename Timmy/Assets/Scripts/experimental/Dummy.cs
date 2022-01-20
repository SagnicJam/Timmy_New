using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Dummy : MonoBehaviour
{
    IObjectPool<Dummy> trackPool;

    public void SetPool(IObjectPool<Dummy> pool)
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
}
