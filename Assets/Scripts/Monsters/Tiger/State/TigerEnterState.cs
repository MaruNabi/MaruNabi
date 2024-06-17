using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TigerEnterState : State<TigerStateMachine>
{
    public TigerEnterState(TigerStateMachine tigerStateMachine) : base(tigerStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        EnterWait().Forget();
    }

    private async UniTaskVoid EnterWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(5f));
        //stateMachine.ChangeAnimation(EMouseAnimationType.StopCrying);
        await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.tiger.SideAtk()));
        stateMachine.SetState("Enter");
    }
}