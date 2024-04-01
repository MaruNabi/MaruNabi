using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MouseFan : MouseState
{
    private const float FAN_DELAY = 0.5f;
    private float elapsedTime;
    private float escapeTime;
    
    private Transform mouseTransform;
    private Vector3 mousePos;
    private GameObject fan;

    public MouseFan(MouseStateMachine stateMachine) : base(stateMachine)
    {
        elapsedTime = 0f;
        escapeTime = 4f;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        mouseTransform = stateMachine.Mouse.transform;
        mousePos = mouseTransform.position;
        AttackWait().Forget();
    }

    async UniTaskVoid AttackWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.GetAnimPlayTime()));
        stateMachine.ChangeAnimation(EAnimationType.Attack);
        await UniTask.Delay(TimeSpan.FromSeconds(FAN_DELAY));
        fan = stateMachine.Mouse.MakeFan(mousePos);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= escapeTime)
        {
            stateMachine.Mouse.DestroyFan(fan);
            stateMachine.SetState("Leave");
        }
    }
}