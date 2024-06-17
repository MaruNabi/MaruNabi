using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TigerSideAttackState : TigerState
{
    public TigerSideAttackState(TigerStateMachine tigerStateMachine) : base(tigerStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("SideAtk1 Enter");
        Pattern(cts.Token).Forget();
    }

    private async UniTask Pattern(CancellationToken token)
    {
        try
        {
            token.ThrowIfCancellationRequested();
            
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.tiger.SideAtk()), cancellationToken: token);
    
            token.ThrowIfCancellationRequested();
            
            stateMachine.SetState("DiagonalAtk");
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Pattern cancelled");
        }
    }
}
