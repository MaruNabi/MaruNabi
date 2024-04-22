using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class MousePhase2State : State<MouseStateMachine>
{
    private Transform mouseTransform;
    
    private float randomPatternPercent;
    private bool isPhase2;
    
    CancellationTokenSource cts;
    
    public MousePhase2State(MouseStateMachine mouseStateMachine) : base(mouseStateMachine)
    {
        randomPatternPercent = 40f;
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        RandomPattern().Forget();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(stateMachine.Mouse.CheckDead())
        {
            cts.Cancel();
            stateMachine.SetState("Dead");
        }
    }
    
    public async UniTaskVoid RandomPattern()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cts.Token);

        switch (stateMachine.Mouse.TakeOne())
        {
            case EMousePattern.HeadButt:
                await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.HeadButt()));
                break;
            case EMousePattern.SpawnRats:
                await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRats()));
                break;
            case EMousePattern.Rock:
                await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRock()));
                break;
            case EMousePattern.Tail:
                await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.TailAttack()));
                break;
        }

        // 연속 공격 확률
        if (RandomizerUtil.PercentRandomizer(randomPatternPercent))
        {
            randomPatternPercent -= 20f;

            if (randomPatternPercent <= 10)
                randomPatternPercent = 10f;

            RandomPattern().Forget();
        }
        else
        {
            stateMachine.SetState("Run");
        }
    }
}
