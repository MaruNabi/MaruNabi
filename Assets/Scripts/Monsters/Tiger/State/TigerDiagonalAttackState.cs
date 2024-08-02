using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TigerDiagonalAttackState : TigerState
{
    public TigerDiagonalAttackState(TigerStateMachine tigerStateMachine) : base(tigerStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Diagonal Enter");
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
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.tiger.DigAtk()), cancellationToken: cts.Token);
            
            stateMachine.SetState("Phase1");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pattern cancelled");
        }
        
    }
}
