using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MouseScholarIdle : MouseScholarState
{
    private float elapsedTime;
    private float escapeTime;
    private bool isBehit;

    public MouseScholarIdle(MouseScholarStateMachine scholarStateMachine) : base(scholarStateMachine)
    {
        elapsedTime = 0f;
        escapeTime = 7f;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        stateMachine.MouseScholar.Idle = true;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= escapeTime)
        {
            isBehit = stateMachine.MouseScholar.IsHit;

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
            
            // Enemy �±� ���ּ� �ǰ� ���·� ��ȯ ����
            stateMachine.MouseScholar.AttackBlocking();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        elapsedTime = 0f;
        stateMachine.MouseScholar.Idle = false;
    }
}