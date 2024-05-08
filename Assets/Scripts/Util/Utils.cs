using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Utils
{
    /// <summary>
    /// 컴포넌트 없으면 Add, 있다면 Get하는 작업을 한 번에 할 수 있도록 하는 함수.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        go.TryGetComponent(out T component);

        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }
    
    /// <summary>
    /// GetChild를 개선한 함수. recursive는 자식의 자식까지 다 찾을 것인지 묻는 것
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }
    
    /// <summary>
    /// FindChild 오버라이딩. 제네릭을 이용한 버전으로 컴포넌트 불러오기 가능.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                { 
                    if (transform.TryGetComponent(out T component))
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
    
    public static T GetDictValue<T>(Dictionary<string, T> dict, string key)
    {
        return dict.TryGetValue(key, out T value) ? value : default(T);
    }
}