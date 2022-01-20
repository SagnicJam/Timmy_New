using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetRandomWeightedUtility
{
    public static int GetWeightedIndex(List<int> weights)
    {
        int total = 0;
        for (int j = 0; j < weights.Count; j++)
        {
            total += weights[j];
        }
        float random = Random.value * total;
        int chooseNumber = 0;
        float add = weights[0];

        while (random > add && chooseNumber < (weights.Count - 1))
        {
            chooseNumber++;
            add += weights[chooseNumber];
        }
        return chooseNumber;
    }
    public static int GetWeightedIndex(int[] weights)
    {
        int total = 0;
        for (int j = 0; j < weights.Length; j++)
        {
            total += weights[j];
        }
        float random = Random.value * total;
        int chooseNumber = 0;
        float add = weights[0];

        while (random > add && chooseNumber < (weights.Length - 1))
        {
            chooseNumber++;
            add += weights[chooseNumber];
        }
        return chooseNumber;
    }
}
