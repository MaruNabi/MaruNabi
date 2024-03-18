using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index > 0)
                name = name.Substring(index + 1);
            
            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }
        
        return Resources.Load<T>(path);
    }

    #region Prefab

    //Resources -> Prefab 폴더에 있는 프리팹을 생성 해주는 함수
    public GameObject Instantiate(string path, Transform parent = null)
    {
        //Resources 산하의 Prefabs 폴더에서 해당 이름에 해당하는 컨텐츠를 찾는다.
        //Resources.Load를 사용해서 가능한 방법
        GameObject original = Load<GameObject>($"Prefab/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        //풀링이 되어 있다면 풀에서 pop해준다. Load를 하지 않고
        if (original.TryGetComponent(out Poolable _))
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    //UI
    public GameObject InstantiateUI(string path, RectTransform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefab/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        //풀링이 되어 있다면 풀에서 pop해준다. Load를 하지 않고
        if (original.TryGetComponent(out Poolable _))
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }


    #endregion

    // public void saveData()
    // {
    //     List<UnlockCard> deckCardsList = new List<UnlockCard>();
    //     
    //     DeckSaveData _decksave = new DeckSaveData();
    //     
    //     foreach (KeyValuePair<int, UnlockCard> cards in Managers.Data.DeckDict)
    //     {
    //         deckCardsList.Add(cards.Value);
    //     }
    //     
    //     _decksave.deckCards = deckCardsList.ToArray();
    //
    //     //인스턴스를 저장할 데이터로 변환
    //     string ToJsonData = JsonUtility.ToJson(_decksave, true);
    //
    //     //저장되는 파일의 주소 -> Application.datPath를 이용하면 프로젝트의 Asset폴더 위치를 찾아줌
    //     string filePath = Application.dataPath + "/Resources/Data/DeckData.json";
    //
    //     File.WriteAllText(filePath, ToJsonData);
    // }
}
