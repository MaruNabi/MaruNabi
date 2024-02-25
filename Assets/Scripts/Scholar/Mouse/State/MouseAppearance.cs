using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAppearance : MouseState
{
    private float elapsedTime = 0f;

    private float escapeTime = 1f;

    public MouseAppearance(MouseStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();

        // TO DO: kimyeonmo 240225 AppearanceCoroutine 추가하기
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        this.elapsedTime += Time.deltaTime;

        if (this.elapsedTime >= this.escapeTime)
        {
            this.stateMachine.SetState("Idle");
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        this.elapsedTime = 0f;
    }
}
