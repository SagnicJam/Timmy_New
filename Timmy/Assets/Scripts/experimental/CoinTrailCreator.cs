using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrailCreator : MonoBehaviour
{
    public float coinSpawnUpHeight=1;
    public float coinSpawnningProbability=1f;
    public int trackLength;
    public int trackLanes;

    CellOccupancy[,] cellOccupancyArr;

    int[] coinWeightsOnTracks;
    /// <summary>
    /// first = spawn,second =notspawn
    /// </summary>
    int[] obstacleSpawnChance;
    /// <summary>
    ///  first = up,second =down
    /// </summary>
    int[] coinSpawnUpOrDownChance;

    int lastSpawnnedTrack=0;
    int lastSpawnnedLane=0;

    public dummyspawnner coinSpawnner;

    public dummyspawnner onexone_spawnner;
    public dummyspawnner onexone_passable_spawnner;
    public dummyspawnner onextwo_spawnner;
    public dummyspawnner twoxone_spawnner;
    public dummyspawnner twoxtwo_spawnner;

    private void Start()
    {
        cellOccupancyArr = new CellOccupancy[trackLength, trackLanes];
        coinWeightsOnTracks = new int[] { 1,1,1};
        obstacleSpawnChance = new int[] { 1,2};
        coinSpawnUpOrDownChance = new int[] { 1,1};

        for (int i = 0; i < trackLength; i++)
        {
            if(Random.value<=coinSpawnningProbability)
            {
                if(lastSpawnnedTrack==i-1)
                {
                    //code check for adjacency
                    if(lastSpawnnedLane==0)
                    {
                        RemoveCoinWeightIndex(2);
                    }
                    else if(lastSpawnnedLane==1)
                    {
                        EqualiseCoinWeights();
                    }
                    else if(lastSpawnnedLane==2)
                    {
                        RemoveCoinWeightIndex(0);
                    }
                }
                else
                {
                    EqualiseCoinWeights();
                    //spawn wherver
                }
                lastSpawnnedTrack = i;
                lastSpawnnedLane = GetRandomWeightedUtility.GetWeightedIndex(coinWeightsOnTracks);
                Dummy dummy=coinSpawnner.Spawn(new Vector3(lastSpawnnedLane, 0, lastSpawnnedTrack));
                cellOccupancyArr[lastSpawnnedTrack, lastSpawnnedLane] = new CellOccupancy(true, false,false,false, new Vector3(lastSpawnnedLane, 0, lastSpawnnedTrack), dummy.gameObject,null);
            }
        }

        for (int i = 0; i < trackLength; i++)
        {
            //check where the coin is placed
            for (int j = 0; j < trackLanes; j++)
            {
                if (ShouldSpawnObstacle())
                {
                    cellOccupancyArr[i, j].hasObstacle = true;
                    Dummy dummy = onexone_spawnner.Spawn(new Vector3(j, 0, i));
                    cellOccupancyArr[i, j].obstacleGO = dummy.gameObject;
                }
            }
        }

        for (int i = 0; i < trackLength; i++)
        {
            for (int j = 0; j < trackLanes; j++)
            {
                if (cellOccupancyArr[i,j].hasCoin&& cellOccupancyArr[i, j].hasObstacle)
                {
                    if(ShouldMoveCoinUp())
                    {
                        cellOccupancyArr[i, j].coinPos += Vector3.up* coinSpawnUpHeight;
                        cellOccupancyArr[i, j].coinGO.transform.position = cellOccupancyArr[i, j].coinPos;
                    }
                    else
                    {
                        cellOccupancyArr[i, j].isObstaclePassable = true;
                        cellOccupancyArr[i, j].obstacleGO.GetComponent<MeshRenderer>().materials[0].color = Color.black;
                        cellOccupancyArr[i, j].coinGO.GetComponent<MeshRenderer>().materials[0].color = Color.green;
                    }
                }
            }
        }


        //for (int i = 0; i < trackLength; i++)
        //{
        //    for (int j = 0; j < trackLanes; j++)
        //    {
        //        if (cellOccupancyArr[i, j].hasObstacle && !cellOccupancyArr[i, j].isObstaclePassable&& !cellOccupancyArr[i, j].isObstacleLinked)
        //        {
        //            bool spawnonextwo=false;
        //            if((i+1)<trackLength)
        //            {
        //                if(cellOccupancyArr[i+1, j].hasObstacle && !cellOccupancyArr[i+1, j].isObstaclePassable && !cellOccupancyArr[i, j].isObstacleLinked)
        //                {
        //                    //keep track for probabilty spawn
        //                    spawnonextwo = true;
        //                }
        //            }
        //            if((j + 1) < trackLanes)
        //            {
        //                if (cellOccupancyArr[i , j+1].hasObstacle && !cellOccupancyArr[i + 1, j].isObstaclePassable && !cellOccupancyArr[i, j].isObstacleLinked)
        //                {
        //                    //keep track for probabilty spawn
        //                    spawnonextwo = true;
        //                }
        //            }
        //        }
        //    }
        //}
    }

    bool ShouldMoveCoinUp()
    {
        return (0 == GetRandomWeightedUtility.GetWeightedIndex(coinSpawnUpOrDownChance));
    }

    bool ShouldSpawnObstacle()
    {
        return (0==GetRandomWeightedUtility.GetWeightedIndex(obstacleSpawnChance));
    }

    //void EqualiseObstacleSpawnWeights()
    //{
    //    for (int i = 0; i < obstacleSpawnChance.Length; i++)
    //    {
    //        obstacleSpawnChance[i] = 1;
    //    }
    //}

    //void DecreaseObstacleSpawnChance()
    //{
    //    for (int i = 0; i < obstacleSpawnChance.Length; i++)
    //    {
    //        if(i==0)
    //        {
    //            obstacleSpawnChance[i] = 1;
    //        }
    //        else if(i==1)
    //        {
    //            obstacleSpawnChance[i] = 0.5f;
    //        }
    //    }
    //}

    void EqualiseCoinWeights()
    {
        for (int i = 0; i < coinWeightsOnTracks.Length; i++)
        {
            coinWeightsOnTracks[i] = 1;
        }
    }

    void RemoveCoinWeightIndex(int index)
    {
        if(index<0)
        {
            Debug.LogError("wrong index "+index);
            return;
        }
        if(index>= coinWeightsOnTracks.Length)
        {
            Debug.LogError("wrong index "+index);
            return;
        }
        for (int i = 0; i < coinWeightsOnTracks.Length; i++)
        {
            if (i == index)
            {
                coinWeightsOnTracks[i] = 0;
            }
            else
            {
                coinWeightsOnTracks[i] = 1;
            }
        }
    }

    struct CellOccupancy
    {
        public GameObject coinGO;
        public GameObject obstacleGO;
        public Vector3 coinPos;
        public bool hasCoin;
        public bool hasObstacle;
        public bool isObstaclePassable;
        public bool isObstacleLinked;

        public CellOccupancy(bool hasCoin, bool hasObstacle,bool isObstaclePassable, bool isObstacleLinked, Vector3 coinPos,GameObject coinGO,GameObject obstacleGO)
        {
            this.hasCoin = hasCoin;
            this.hasObstacle = hasObstacle;
            this.isObstaclePassable = isObstaclePassable;
            this.coinPos = coinPos;
            this.coinGO = coinGO;
            this.obstacleGO = obstacleGO;
            this.isObstacleLinked = isObstacleLinked;
        }
    }
}
