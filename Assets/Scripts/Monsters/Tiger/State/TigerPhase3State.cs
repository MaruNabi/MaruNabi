using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TigerPhase3State : TigerState
{
    public TigerPhase3State(TigerStateMachine tigerStateMachine) : base(tigerStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        IdleWait().Forget();
    }

    private async UniTaskVoid IdleWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(5f));
        // 페이즈 3 패턴
    }
}