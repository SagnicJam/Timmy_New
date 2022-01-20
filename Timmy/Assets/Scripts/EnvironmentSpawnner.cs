using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawnner : MonoBehaviour
{
    public List<EnvironmentData> environmentDatas;
}
[System.Serializable]
public struct EnvironmentData
{
    public int weight;
    public GameObject go;
}
