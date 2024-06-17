using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearTimer : MonoBehaviour
{
    private float startTime;
    public float CurrentTime { get; private set; }
    public bool IsEnd { get; set; }

    void Start()
    {
        startTime = Time.time;
        CurrentTime = 0.0f;
        IsEnd = false;
    }

    void Update()
    {
        if (IsEnd)
            return;

        TimeCheck();
    }

    private void TimeCheck()
    {
        CurrentTime = Time.time - startTime;
    }
}
