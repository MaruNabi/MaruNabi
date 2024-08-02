using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TigerBiteAttackState : TigerState
{
    public TigerBiteAttackState(TigerStateMachine tigerStateMachine) : base(tigerStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        
        cts = new CancellationTokenSource();
        
        Pattern(cts.Token).Forget();
        Debug.Log("물기입장");
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
            token.ThrowIfCancellationRequested();
            
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.tiger.Bite()), cancellationToken: cts.Token);
    
            token.ThrowIfCancellationRequested();
            
            stateMachine.SetState("Phase2");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pattern cancelled");
        }
    }
}