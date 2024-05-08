using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MouseScholarLeave : MouseScholarState
{
    private const float ESCAPE_TIME = 2f;
    
    public MouseScholarLeave(MouseScholarStateMachine scholarStateMachine) : base(scholarStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        ExitDelay().Forget();
    }

    async UniTaskVoid ExitDelay()
    {
        if (stateMachine.MouseScholar.Dead == false)
        {
            // 피격 상태일 경우 대기
            if(stateMachine.MouseScholar.IsHit)
                await UniTask.Delay(TimeSpan.FromSeconds(4f));

            stateMachine.MouseScholar.SmokeEffect();
            // 재입장까지 대기 시간
            await UniTask.Delay(TimeSpan.FromSeconds(ESCAPE_TIME));
            
            stateMachine.MouseScholar.RoundEnd();
            stateMachine.MouseScholar.OnDead();
        }
    }
}