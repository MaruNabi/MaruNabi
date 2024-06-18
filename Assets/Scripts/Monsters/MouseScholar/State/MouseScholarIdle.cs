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

        stateMachine.MouseScholar.IsIdle = true;
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
                stateMachine.ChangeAnimation(EScholarAnimationType.Hit);
                Managers.Sound.PlaySFX("Scholar_Angry");
            }
            else
            {
                stateMachine.SetState("Fan");
                stateMachine.ChangeAnimation(EScholarAnimationType.Laugh);
                Managers.Sound.PlaySFX("Scholar_Laugh");
            }
            
            // Enemy 태그 없애서 피격 상태로 전환 방지
            stateMachine.MouseScholar.AttackBlocking();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        elapsedTime = 0f;
        stateMachine.MouseScholar.IsIdle = false;
    }
}