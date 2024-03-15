using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class MonsterData
{
    public int ID;
    public string Code;
    public string name;
    public int MonsterType;
    public bool MovementType;
    public int Passive;
}

[Serializable]
public class MonsterDict : ILoader<int, MonsterData>
{
    // 받아온 정보를 모두 리스트에 저장하는데 이 리스트의 이름은 json에 적힌 이름과 같아야 한다
    public List<MonsterData> monsterDatas = new List<MonsterData>();

    //json으로부터 받아온 정보를 기반으로 딕셔너리를 구성한다
    public Dictionary<int, MonsterData> MakeDict()
    {
        Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
        foreach (MonsterData card in monsterDatas)
        {
            dict.Add(card.ID, card);
        }
        return dict;
    }
}
