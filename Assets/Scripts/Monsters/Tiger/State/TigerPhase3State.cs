using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TigerPhase3State : TigerState
{
    public TigerPhase3State(TigerStateMachine tigerStateMachine) : base(tigerStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    { 
        cts = new CancellationTokenSource();
        
        base.OnEnter();
        IdleWait(cts.Token).Forget();
    }
    
    public override void OnExit()
    {
        base.OnExit();
        cts.Cancel();
    }

    private async UniTaskVoid IdleWait(CancellationToken token)
    {
        if(stateMachine.tiger.CheckMiniPhaseChangeHP())
        {
            // 손 내려치기 or 빨아들이기
            int random = UnityEngine.Random.Range(0, 2);
            if(random == 0)
            {
                stateMachine.SetState("SlapAtk");
            }
            else
            {
                stateMachine.SetState("InhaleAtk");
            }
        }
        else
        {
            stateMachine.SetState("SideAtk2");
        }
        // 페이즈 3 패턴
    }
}