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
        base.OnEnter();
        IdleWait().Forget();
    }

    private async UniTaskVoid IdleWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(4f));
        stateMachine.SetState("BiteAtk");
    }
}