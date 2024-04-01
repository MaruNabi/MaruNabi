using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MouseLeave : MouseState
{
    private const float ESCAPE_TIME = 2f;
    
    public MouseLeave(MouseStateMachine stateMachine) : base(stateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        ExitDelay().Forget();
    }

    async UniTaskVoid ExitDelay()
    {
        // 피격 상태일 경우 대기
        if(stateMachine.Mouse.IsHit)
            await UniTask.Delay(TimeSpan.FromSeconds(4f));
        
        stateMachine.Mouse.SmokeEffect();
        // 재입장까지 대기 시간
        await UniTask.Delay(TimeSpan.FromSeconds(ESCAPE_TIME));
        
        stateMachine.Mouse.RoundEnd();
        
        if (stateMachine.Mouse.HP <= 0 && stateMachine.Mouse.Dead == false)
        {
            // 스테이지 종료
            stateMachine.Mouse.Death();
        }
        
        stateMachine.Mouse.Leave();
    }
}
