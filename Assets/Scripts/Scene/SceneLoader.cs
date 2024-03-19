using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }
    public void LoadScene(ESceneType type)
    {
        //씬을 로드할 때 이미 생성된 데이터들을 싹 날리고 로드한다.
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
    }

    //현재 씬의 이름을 알 수 있는 함수
    string GetSceneName(ESceneType type)
    {
        string name = System.Enum.GetName(typeof(ESceneType), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
