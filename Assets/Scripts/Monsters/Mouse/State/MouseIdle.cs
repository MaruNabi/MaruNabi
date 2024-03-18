using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIdle : MouseState
{
    private float elapsedTime = 0f;

    private float escapeTime = 5f;

    private bool isBehit;
    public MouseIdle(MouseStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        this.stateMachine.Mouse.IsIdle = true;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        this.elapsedTime += Time.deltaTime;

        if (this.elapsedTime >= this.escapeTime)
        {
            Debug.Log("mouse idle 시간 달성");

            this.isBehit = this.stateMachine.Mouse.scholarManager.GetIsSchloarBehit();

            if (this.isBehit == true)
            {
                this.stateMachine.SetState("Fan");
            }
        }
    }
    public override void OnExit()
    {
        base.OnExit();

        this.elapsedTime = 0f;
        this.stateMachine.Mouse.IsIdle = false;
    }
}
