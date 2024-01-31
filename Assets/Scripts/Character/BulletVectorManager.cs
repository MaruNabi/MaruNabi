using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LockedKeys
{
    Left,
    Top,
    Right,
    Bottom,
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft
}

public class BulletVectorManager : MonoBehaviour
{
    Dictionary<KeyCode, LockedKeys> keyDictionary;

    Vector2 bulletVector;

    void Start()
    {
        keyDictionary = new Dictionary<KeyCode, LockedKeys>
        {
            { KeyCode.LeftArrow, LockedKeys.Left },
            { KeyCode.UpArrow, LockedKeys.Top },
            { KeyCode.RightArrow, LockedKeys.Right },
            { KeyCode.DownArrow, LockedKeys.Bottom },
        };
    }

    void Update()
    {
        if (Input.anyKey)
        {
            foreach (var dic in keyDictionary)
            {
                if (Input.GetKey(dic.Key))
                {
                    if (dic.Value == LockedKeys.Left && dic.Value == LockedKeys.Top)
                    {
                        Debug.Log("TopLeft");
                    }
                    else if (dic.Value == LockedKeys.Left)
                    {
                        Debug.Log("Left");
                    }
                    else if (dic.Value == LockedKeys.Top)
                    {
                        Debug.Log("Top");
                    }
                    
                }
            }
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("TopLeft");
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Left");
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Top");
        }
    }
}
