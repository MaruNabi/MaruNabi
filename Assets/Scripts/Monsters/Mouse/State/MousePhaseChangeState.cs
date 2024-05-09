using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MousePhaseChangeState : State<MouseStateMachine>
{
    private Transform mouseTransform;
    private const float RUNNNING_TIME = 4f;
    
    public MousePhaseChangeState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.Mouse.PhaseChange = true;
        stateMachine.Mouse.tag = "NoDeleteEnemyBullet";
        Mouse.MovingBackGround?.Invoke(false);
        stateMachine.ChangeAnimation(EMouseAnimationType.Dead);
        stateMachine.Mouse.AllowAttack(false);
        stateMachine.Mouse.PhaseChangeAnim();
        PhaseChangeWait().Forget();
    }
    
    async UniTaskVoid PhaseChangeWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(RUNNNING_TIME));
        stateMachine.SetState("Run");
    }
}
