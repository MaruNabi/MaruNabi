using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MouseEnterState : State<MouseStateMachine>
{
    private Transform mouseTransform;

    public MouseEnterState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        mouseTransform = stateMachine.Mouse.transform;
        EnterWait().Forget();
    }

    private async UniTaskVoid EnterWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        mouseTransform.DOShakeScale(2f,0.25f,5);
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        stateMachine.SetState("Run");
    }
}
