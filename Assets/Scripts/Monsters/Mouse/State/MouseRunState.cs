using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MouseRunState : MouseState
{
    public MouseRunState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Run");
        RunWait().Forget();
    }

    async UniTaskVoid RunWait()
    {
        stateMachine.Mouse.BackGroundControll(true);
        stateMachine.ChangeAnimation(EMouseAnimationType.Run);

        await UniTask.Delay(TimeSpan.FromSeconds(2f));

        if (stateMachine.Mouse.PhaseChange)
            stateMachine.SetState("Phase2");
        else
            stateMachine.SetState("Phase1");
    }
}