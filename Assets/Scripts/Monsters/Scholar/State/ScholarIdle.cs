using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ScholarIdle : ScholarState
{
    private float elapsedTime;
    private float escapeTime;
    private bool isBehit;

    public ScholarIdle(ScholarStateMachine stateMachine) : base(stateMachine)
    {
        elapsedTime = 0f;
        escapeTime = 7f;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.Scholar.Idle = true;
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= escapeTime)
        {
            isBehit = stateMachine.Scholar.IsHit;

            if (isBehit)
            {
                stateMachine.SetState("Fan");
                stateMachine.ChangeAnimation(EAnimationType.Angry);
            }
            else
            {
                stateMachine.SetState("Leave");
            }
            
            stateMachine.Scholar.AttackBlocking();
        }
    }
    
    public override void OnExit()
    {
        base.OnExit();
        elapsedTime = 0f;
        stateMachine.Scholar.Idle = false;
    }
}
