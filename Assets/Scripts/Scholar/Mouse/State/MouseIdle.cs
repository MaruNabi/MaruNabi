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

        Debug.Log("idle �ð�: " + elapsedTime);
        this.elapsedTime += Time.deltaTime;

        if (this.elapsedTime >= this.escapeTime)
        {
            Debug.Log("idle �ð� �޼�");

            this.isBehit = this.stateMachine.Mouse.mouseManager.GetIsSchloarBehit();

            if (this.isBehit == true)
            {
                // TO DO: ���� Fan�� ����
                // this.stateMachine.SetState("Fan");
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
