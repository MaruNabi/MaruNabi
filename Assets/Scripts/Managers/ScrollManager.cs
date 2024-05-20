using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour, IScroller
{
    [SerializeField] protected float speed;
    protected bool isStart;

    public void SetIsStart(bool _isStart)
    {
        isStart = _isStart;
    }

    public void SetSpeed(float _speed)
    {
        speed += _speed;
    }
}