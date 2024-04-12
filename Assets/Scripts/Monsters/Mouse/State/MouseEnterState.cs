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
        mouseTransform.DOShakeScale(2f);
        stateMachine.SetState("Run");
    }
}
