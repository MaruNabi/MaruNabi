using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TigerSideAttack2State : TigerState
{
    public TigerSideAttack2State(TigerStateMachine tigerStateMachine) : base(tigerStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Pattern(cts.Token).Forget();
        Debug.Log("�ܱ� 2 ����");
    }

    private async UniTask Pattern(CancellationToken token)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.tiger.SideAtk2()), cancellationToken: token);
    
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