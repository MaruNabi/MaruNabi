using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TigerPhase1State : TigerState
{
    public TigerPhase1State(TigerStateMachine tigerStateMachine) : base(tigerStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        
        cts = new CancellationTokenSource();
        
        Debug.Log("Phase1");
        
        IdleWait(cts.Token).Forget();
    }
    
    public override void OnExit()
    {
        base.OnExit();
        cts.Cancel();
    }

    private async UniTaskVoid IdleWait(CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(4f) , cancellationToken: cts.Token);
        stateMachine.SetState("SideAtk1");
    }
}