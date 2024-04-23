using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarAppearance : ScholarState
{
    private float elapsedTime;
    private float escapeTime;

    public ScholarAppearance(ScholarStateMachine stateMachine) : base(stateMachine)
    {
        elapsedTime = 0f;
        escapeTime = 1f;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.Scholar.AppearanceEffect();
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