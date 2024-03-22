using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class MonsterData
{
    public string CODE;
    public int ID;
    public string NAME;
    public int TYPE;
    public bool MOVETYPE;
    public float LIFE;
    public float SPEED;
    public float JUMPPOWER;
    public int PASSIVE;
    public float RESPAWNTIME;
}

[Serializable]
public class MonsterDict : ILoader<string, MonsterData>
{
    // 받아온 정보를 모두 리스트에 저장하는데 이 리스트의 이름은 json에 적힌 이름과 같아야 한다
    public List<MonsterData> Data = new List<MonsterData>();

    //json으로부터 받아온 정보를 기반으로 딕셔너리를 구성한다
    public Dictionary<string, MonsterData> MakeDict()
    {
        Dictionary<string, MonsterData> dict = new Dictionary<string, MonsterData>();
        foreach (MonsterData item in Data)
        {
            dict.Add(item.CODE, item);
        }
        return dict;
    }
}
