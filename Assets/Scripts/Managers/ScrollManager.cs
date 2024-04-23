using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour, IScroller
{
    protected bool isStart;

    public void SetIsStart(bool _isStart)
    {
        isStart = _isStart;
    }
}
