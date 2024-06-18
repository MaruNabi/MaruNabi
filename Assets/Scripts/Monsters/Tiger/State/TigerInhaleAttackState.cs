using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TigerInhaleAttackState : TigerState
{
    public TigerInhaleAttackState(TigerStateMachine tigerStateMachine) : base(tigerStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Pattern(cts.Token).Forget();
        Debug.Log("빨아들이기 입장");
    }
    
    public override void OnExit()
    {
        base.OnExit();
        cts.Cancel();
    }

    private async UniTask Pattern(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.tiger.Inhale()), cancellationToken: token);
    
            token.ThrowIfCancellationRequested();
            
            await UniTask.Delay(TimeSpan.FromSeconds(6f), cancellationToken: token);
            
            token.ThrowIfCancellationRequested();

            stateMachine.SetState("Phase3");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pattern cancelled");
        }
    }
}