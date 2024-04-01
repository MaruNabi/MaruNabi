using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MouseIdle : MouseState
{
    private float elapsedTime;
    private float escapeTime;
    private bool isBehit;

    public MouseIdle(MouseStateMachine stateMachine) : base(stateMachine)
    {
        elapsedTime = 0f;
        escapeTime = 7f;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        stateMachine.Mouse.Idle = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= escapeTime)
        {
            isBehit = stateMachine.Mouse.IsHit;

            if (isBehit)
            {
                stateMachine.SetState("Leave");
                stateMachine.ChangeAnimation(EAnimationType.Hit);
            }
            else
            {
                stateMachine.SetState("Fan");
                stateMachine.ChangeAnimation(EAnimationType.Laugh);
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        elapsedTime = 0f;
        stateMachine.Mouse.Idle = false;
    }
}
