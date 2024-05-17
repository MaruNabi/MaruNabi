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
        RandomPattern(cts.Token).Forget();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateMachine.Mouse.CheckDead() && stateMachine.Mouse.Dead == false)
        {
            cts.Cancel();
            stateMachine.Mouse.OnDead();
        }
    }

    private async UniTask RandomPattern(CancellationToken token)
    {

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

            switch (stateMachine.Mouse.TakeOne())
            {
                case EMousePattern.Rush:
                    token.ThrowIfCancellationRequested();
                    Mouse.MovingBackGround?.Invoke(false);
                    await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.Rush()), cancellationToken: token);
                    token.ThrowIfCancellationRequested();
                    Mouse.MovingBackGround?.Invoke(true);
                    break;
                case EMousePattern.Tail:
                    token.ThrowIfCancellationRequested();
                    Mouse.MovingBackGround?.Invoke(false);
                    await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.TailAttack()), cancellationToken: token);
                    token.ThrowIfCancellationRequested();
                    Mouse.MovingBackGround?.Invoke(true);
                    break;
                case EMousePattern.SpawnRats:
                    stateMachine.ChangeAnimation(EMouseAnimationType.Crying);
                    await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRats()), cancellationToken: token);
                    break;
                case EMousePattern.Rock:
                    stateMachine.ChangeAnimation(EMouseAnimationType.Crying);
                    await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRock()), cancellationToken: token);
                    break;
            }
            
            token.ThrowIfCancellationRequested();
            
            if (RandomizerUtil.PercentRandomizer(stateMachine.Mouse.PatternPercent))
            {
                stateMachine.Mouse.MinusRandomPecent(20f);

                await RandomPattern(token);
            }
            else
            {
                stateMachine.SetState("Run");
            }
        }
        catch (OperationCanceledException)
        {
            // Handle the cancellation if needed
            Debug.Log("RandomPattern cancelled");
        }
    }
}
