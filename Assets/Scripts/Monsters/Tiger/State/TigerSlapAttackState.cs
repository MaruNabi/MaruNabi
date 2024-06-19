using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TigerSlapAttackState : TigerState
{
    public TigerSlapAttackState(TigerStateMachine tigerStateMachine) : base(tigerStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        cts = new CancellationTokenSource();

        Pattern(cts.Token).Forget();
    }

    public override void OnExit()
    {
        base.OnExit();
        cts.Cancel();
    }


    private async UniTaskVoid Pattern(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.tiger.SlapAtk()), cancellationToken: token);

            token.ThrowIfCancellationRequested();

            await UniTask.Delay(TimeSpan.FromSeconds(4f), cancellationToken: token);

            token.ThrowIfCancellationRequested();

            stateMachine.SetState("Phase3");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pattern cancelled");
        }
    }
}