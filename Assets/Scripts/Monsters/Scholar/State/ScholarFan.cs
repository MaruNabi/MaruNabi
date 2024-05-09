using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ScholarFan : ScholarState
{
    private const float ATTACK_DELAY = 0.5f;
    private float elapsedTime;
    private float escapeTime;

    private Transform scholarTransform;
    private Vector3 scholarPos;
    private GameObject fan;
    
    public ScholarFan(ScholarStateMachine stateMachine) : base(stateMachine)
    {
        elapsedTime = 0f;
        escapeTime = 4f;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        scholarTransform = stateMachine.Scholar.transform;
        scholarPos = scholarTransform.position;
        AttackWait().Forget();
    }
    
    private async UniTaskVoid AttackWait()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.GetAnimPlayTime()));
        stateMachine.ChangeAnimation(EScholarAnimationType.Attack);
        
        await UniTask.Delay(TimeSpan.FromSeconds(ATTACK_DELAY));
        fan = stateMachine.Scholar.MakeFan(scholarPos);
        Entity.AttackEvent?.Invoke();
    }
    
    public override void OnUpdate()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= escapeTime)
        {
            stateMachine.Scholar.DestroyFan(fan);
            stateMachine.SetState("Leave");
        }
    }
}
