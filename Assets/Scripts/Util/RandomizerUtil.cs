using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomizerUtil
{
    public static WeightedRandomizer<R> From<R>(Dictionary<R, int> spawnRate)
    {
        return new WeightedRandomizer<R>(spawnRate);
    }
    
    public static bool PercentRandomizer(float probability)
    {
        float percentage = probability / 100;
        float rate = 100 - (100 * percentage);
        int tmp = Random.Range(0, 100);

        if (tmp <= rate - 1)
        {
            return false;
        }

        return true;
    }
}

public class WeightedRandomizer<T>
{
    private static System.Random _random = new System.Random();
    private Dictionary<T, int> _weights;

    public WeightedRandomizer(Dictionary<T, int> weights)
    {
        _weights = weights;
    }

    public T TakeOne()
    {
        var sortedSpawnRate = Sort(_weights);
        int sum = 0;
        foreach (var spawn in _weights)
        {
            sum += spawn.Value;
        }

        int roll = _random.Next(0, sum);

        T selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
        foreach (var spawn in sortedSpawnRate)
        {
            if (roll < spawn.Value)
            {
                selected = spawn.Key;
                break;
            }
            roll -= spawn.Value;
        }

        return selected;
    }

    private List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
    {
        var list = new List<KeyValuePair<T, int>>(weights);

        list.Sort(
            delegate (KeyValuePair<T, int> firstPair,
                        KeyValuePair<T, int> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
            );

        return list;
    }
}