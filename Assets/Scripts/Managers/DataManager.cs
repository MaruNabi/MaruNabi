using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{
    public Dictionary<int, MonsterData> monsterDict { get; private set; } = new Dictionary<int, MonsterData>();
    
    public void Init()
    {
        //Resources의 Data 폴더 산하에서 해당하는 이름에 맞는 json 데이터를 읽어온다.
        monsterDict = LoadJson<MonsterDict, int, MonsterData>("MonsterData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        //json파일은 텍스트 형태로 읽어온다.
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}