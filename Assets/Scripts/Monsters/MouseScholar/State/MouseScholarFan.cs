using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MouseScholarFan : MouseScholarState
{
    private const float FAN_DELAY = 0.5f;
    private float elapsedTime;
    private float escapeTime;
    
    private Transform mouseTransform;
    private Vector3 mousePos;
    private GameObject fan;

    public MouseScholarFan(MouseScholarStateMachine scholarStateMachine) : base(scholarStateMachine)
    {
        elapsedTime = 0f;
        escapeTime = 4f;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        mouseTransform = stateMachine.MouseScholar.transform;
        mousePos = mouseTransform.position;
        AttackWait().Forget();
    }

    async UniTaskVoid AttackWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.GetAnimPlayTime()));
        stateMachine.ChangeAnimation(EScholarAnimationType.Attack);
        await UniTask.Delay(TimeSpan.FromSeconds(FAN_DELAY));
        fan = stateMachine.MouseScholar.MakeFan(mousePos);
        Entity.AttackEvent?.Invoke();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= escapeTime)
        {
            stateMachine.MouseScholar.DestroyFan(fan);
            stateMachine.SetState("Leave");
        }
    }
}