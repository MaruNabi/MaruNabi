using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TigerPhase2State : TigerState
{
    public TigerPhase2State(TigerStateMachine tigerStateMachine) : base(tigerStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    { 
        cts = new CancellationTokenSource();
        
        base.OnEnter();
        IdleWait(cts.Token).Forget();
    }
    
    public override void OnExit()
    {
        base.OnExit();
        cts.Cancel();
    }

    private async UniTaskVoid IdleWait(CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(4f), cancellationToken: cts.Token);
        stateMachine.SetState("BiteAtk");
    }
}