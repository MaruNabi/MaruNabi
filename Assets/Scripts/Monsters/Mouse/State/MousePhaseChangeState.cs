using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MousePhaseChangeState : State<MouseStateMachine>
{
    private Transform mouseTransform;
    private const float RUNNNING_TIME = 3f;
    
    public MousePhaseChangeState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        PhaseChangeWait().Forget();
    }
    
    async UniTaskVoid PhaseChangeWait()
    {
        stateMachine.Mouse.AllowAttack(false);
        stateMachine.Mouse.PhaseChange = true;
        stateMachine.Mouse.PhaseChangeSprite();
        
        await UniTask.Delay(TimeSpan.FromSeconds(RUNNNING_TIME));
        stateMachine.SetState("Run");
    }
}
