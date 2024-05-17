using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MousePhaseChangeState : State<MouseStateMachine>
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
        Mouse.MovingBackGround?.Invoke(false);
        await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.PhaseChangeSequence()-1f));
        stateMachine.SetState("Run");
    }
}
