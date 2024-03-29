using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarIdle : ScholarState
{
    private float elapsedTime = 0f;

    private float escapeTime = 5f;

    private bool isBehit;
    public ScholarIdle(ScholarStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        this.stateMachine.Scholar.IsIdle = true;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

        this.elapsedTime += Time.deltaTime;

        if (this.elapsedTime >= this.escapeTime)
        {
            //Debug.Log("idle 시간 달성");

            this.isBehit = this.stateMachine.Scholar.scholarManager.GetIsSchloarBehit();

            if (this.isBehit == true)
            {
                this.stateMachine.SetState("Fan");
                
                // 공격 애니메이션
                stateMachine.ChangeAnimationAttack();
            }
        }
    }
    public override void OnExit()
    {
        base.OnExit();

        this.elapsedTime = 0f;
        this.stateMachine.Scholar.IsIdle = false;
    }
}
