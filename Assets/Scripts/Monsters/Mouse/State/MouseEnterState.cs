using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MouseEnterState : State<MouseStateMachine>
{
    public MouseEnterState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        EnterWait().Forget();
    }

    private async UniTaskVoid EnterWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        stateMachine.ChangeAnimation(EMouseAnimationType.StopCrying);
        await UniTask.Delay(TimeSpan.FromSeconds(0.6f));
        stateMachine.SetState("Run");
    }
}