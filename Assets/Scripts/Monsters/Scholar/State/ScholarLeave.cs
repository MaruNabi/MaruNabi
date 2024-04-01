using System;
using Cysharp.Threading.Tasks;

public class ScholarLeave : ScholarState
{
    private const float ESCAPE_TIME = 2f;
    
    public ScholarLeave(ScholarStateMachine stateMachine) : base(stateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        ExitDelay().Forget();
    }
    
    async UniTaskVoid ExitDelay()
    {
        stateMachine.Scholar.SmokeEffect();
        // 재입장까지 대기 시간
        await UniTask.Delay(TimeSpan.FromSeconds(ESCAPE_TIME));
        stateMachine.Scholar.Leave();
    }
}
