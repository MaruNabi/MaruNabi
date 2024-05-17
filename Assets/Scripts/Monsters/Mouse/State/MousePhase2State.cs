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
    CancellationTokenSource cts;

    public MousePhase2State(MouseStateMachine mouseStateMachine) : base(mouseStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.Mouse.PatternPercent = 40f;
        Debug.Log("Phase2");
        RandomPattern().Forget();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateMachine.Mouse.CheckDead() && stateMachine.Mouse.Dead == false)
        {
            stateMachine.SetState("Dead");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        cts.Cancel();
    }

    public async UniTaskVoid RandomPattern()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cts.Token);

        switch (stateMachine.Mouse.TakeOne())
        {
            case EMousePattern.Rush:
                Mouse.MovingBackGround?.Invoke(false);
                await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.Rush()), cancellationToken: cts.Token);
                Mouse.MovingBackGround?.Invoke(true);
                break;
            case EMousePattern.Tail:
                Mouse.MovingBackGround?.Invoke(false);
                await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.TailAttack()), cancellationToken: cts.Token);
                Mouse.MovingBackGround?.Invoke(true);
                break;
            case EMousePattern.SpawnRats:
                stateMachine.ChangeAnimation(EMouseAnimationType.Crying);
                await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRats()), cancellationToken: cts.Token);
                break;
            case EMousePattern.Rock:
                stateMachine.ChangeAnimation(EMouseAnimationType.Crying);
                await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRock()), cancellationToken: cts.Token);
                break;
        }
        if (RandomizerUtil.PercentRandomizer(stateMachine.Mouse.PatternPercent))
        {
            Debug.Log("레츠고2");
            stateMachine.SetState("Serise");
        }
        else
        {
            stateMachine.SetState("Run");
        }
    }
}
