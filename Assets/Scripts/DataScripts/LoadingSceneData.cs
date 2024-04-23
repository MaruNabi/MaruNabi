using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class LoadingSceneData
{
    public int CODE;
    public string DIALOGUE;
}

[Serializable]
public class LoadingSceneDict : ILoader<int, LoadingSceneData>
{
    public List<LoadingSceneData> Data = new List<LoadingSceneData>();

    public Dictionary<int, LoadingSceneData> MakeDict()
    {
        Dictionary<int, LoadingSceneData> dict = new Dictionary<int, LoadingSceneData>();
        foreach (LoadingSceneData item in Data)
        {
            dict.Add(item.CODE, item);
        }
        return dict;
    }
}
