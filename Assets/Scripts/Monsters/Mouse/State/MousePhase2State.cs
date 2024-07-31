using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class MousePhase2State : MouseState
{
    public MousePhase2State(MouseStateMachine mouseStateMachine) : base(mouseStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.Mouse.PatternPercent = 40f;
        RandomPattern(cts.Token).Forget();
        
        stateMachine.Mouse.SaveCtsMouse2State(this);
    }


    private async UniTask RandomPattern(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);

            var state = stateMachine.Mouse.TakeOne();
            
            //TODO : 랜덤 패턴 원상복구 필요
            switch (EMousePattern.Tail)
            {
                case EMousePattern.Rush:
                    await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.Rush2()), cancellationToken: token);
                    token.ThrowIfCancellationRequested();
                    break;
                case EMousePattern.Tail:
                    await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.TailAttack()), cancellationToken: token);
                    token.ThrowIfCancellationRequested();
                    break;
                case EMousePattern.SpawnRats:
                    stateMachine.ChangeAnimation(EMouseAnimationType.Crying);
                    await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRats()), cancellationToken: token);
                    token.ThrowIfCancellationRequested();
                    break;
                case EMousePattern.Rock:
                    stateMachine.ChangeAnimation(EMouseAnimationType.Crying);
                    await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRock()), cancellationToken: token);
                    token.ThrowIfCancellationRequested();
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
