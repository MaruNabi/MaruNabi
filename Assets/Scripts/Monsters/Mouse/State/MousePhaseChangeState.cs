using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MousePhaseChangeState : State<MouseStateMachine>
{
    private const float RUNNNING_TIME = 2.75f;
    
    public MousePhaseChangeState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("PhaseChange");
        stateMachine.Mouse.PhaseChangeSequence();
        PhaseChangeWait().Forget();
    }
    
    async UniTaskVoid PhaseChangeWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(RUNNNING_TIME));
        stateMachine.SetState("Run");
    }
}
