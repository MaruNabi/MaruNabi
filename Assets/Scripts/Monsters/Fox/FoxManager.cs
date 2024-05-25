using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class FoxManager : MonoBehaviour
{
    [SerializeField] private StageSwitchingManager stageSwitchingManager;
    [SerializeField] private SoulProduction soul;
    [SerializeField] private NextStageWall nextStageWall;
    [SerializeField] private Fox fox;

    private void Start()
    {
        Fox.Stage3Clear += StageClear;
    }
    
    private void OnDestroy()
    {
        Fox.Stage3Clear -= StageClear;
    }
    
    public async UniTaskVoid StageStart()
    {
        stageSwitchingManager.DisAllowBehavior();
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        soul.gameObject.SetActive(true);
        soul.StartProduction();
        await UniTask.Delay(TimeSpan.FromSeconds(7f));
        fox.ChangeAnimation(EFoxAnimationType.Laugh);
        stageSwitchingManager.AllowBehavior();
    }

    public void StageClear()
    {
        fox.transform.DOShakePosition(1f);
        soul.ClearProduction();
    }
}