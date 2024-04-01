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
        // �ǰ� ������ ��� ���
        if(stateMachine.Mouse.IsHit)
            await UniTask.Delay(TimeSpan.FromSeconds(4f));
        
        stateMachine.Mouse.SmokeEffect();
        // ��������� ��� �ð�
        await UniTask.Delay(TimeSpan.FromSeconds(ESCAPE_TIME));
        
        stateMachine.Mouse.RoundEnd();
        
        if (stateMachine.Mouse.HP <= 0 && stateMachine.Mouse.Dead == false)
        {
            // �������� ����
            stateMachine.Mouse.Death();
        }
        
        stateMachine.Mouse.Leave();
    }
}
