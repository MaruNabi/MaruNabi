using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MousePhaseChangeState : MouseState
{
    public MousePhaseChangeState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("PhaseChange");
        PhaseChangeWait().Forget();
    }
    
    async UniTaskVoid PhaseChangeWait()
    {
        stateMachine.Mouse.BackGroundControl(false);
        await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.PhaseChangeSequence()));
        stateMachine.SetState("Run");
    }
}
