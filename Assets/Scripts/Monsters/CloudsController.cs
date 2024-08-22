using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CloudsController : MonoBehaviour
{
    private CloudPlatform[] clouds;

    private void Awake()
    {
        clouds = GetComponentsInChildren<CloudPlatform>();
    }

    public void DisapCloud(int num)
    {
        if(clouds[num].gameObject.activeSelf == false)
            return;
        clouds[num].DisableSequence();
        DelayActive(num).Forget();
    }
    
    public void AppearCloud(int num)
    {
        clouds[num].gameObject.SetActive(true);
    }
    
    private async UniTaskVoid DelayActive(int num)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(4f));
        AppearCloud(num);
    }
}