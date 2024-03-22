using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LoadingSceneData
{
    public string CODE;
    public string DIALOGUE;
}

public class LoadingDataDict : ILoader<string, LoadingSceneData>
{
    public List<LoadingSceneData> Data = new List<LoadingSceneData>();

    public Dictionary<string, LoadingSceneData> MakeDict()
    {
        Dictionary<string, LoadingSceneData> dict = new Dictionary<string, LoadingSceneData>();
        foreach (LoadingSceneData item in Data)
        {
            dict.Add(item.CODE, item);
        }
        return dict;
    }
}
