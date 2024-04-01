using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseAppearance : MouseState
{
    private float elapsedTime;
    private float escapeTime = 1f;

    public MouseAppearance(MouseStateMachine stateMachine) : base(stateMachine) { }
    
    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.Mouse.AppearanceEffect();
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= escapeTime)
        {
            stateMachine.SetState("Idle");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        elapsedTime = 0f;
    }
}
