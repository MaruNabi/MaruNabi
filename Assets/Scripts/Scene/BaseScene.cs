using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public ESceneType SceneType { get; protected set; } = ESceneType.TitleScene;
    
    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            //새로운 씬에 들어왔을 때, 이벤트 시스템이 존재하지 않을 경우 추가해준다.
            Managers.Resource.Instantiate("EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}