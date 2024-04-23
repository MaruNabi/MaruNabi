using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MouseDeadState : State<MouseStateMachine>
{
    private Transform mouseTransform;
    private const float RUNNNING_TIME = 2f;
    
    public MouseDeadState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.Mouse.OnDead();
    }
}
