using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseScholarScholarAppearance : MouseScholarState
{
    private float elapsedTime;
    private float escapeTime = 1f;

    public MouseScholarScholarAppearance(MouseScholarStateMachine mouseScholarStateMachine) : base(mouseScholarStateMachine) { }
    
    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.MouseScholar.AppearanceEffect();
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
