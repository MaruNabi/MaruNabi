using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TigerPhase1State : TigerState
{
    public TigerPhase1State(TigerStateMachine tigerStateMachine) : base(tigerStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        IdleWait().Forget();
    }

    private async UniTaskVoid IdleWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(5f));
        stateMachine.SetState("SideAtk1");
    }
}