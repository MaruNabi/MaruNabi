using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class MousePhase1State : State<MouseStateMachine>
{
    CancellationTokenSource cts;
    
    public MousePhase1State(MouseStateMachine mouseStateMachine) : base(mouseStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.Mouse.PatternPercent = 30f;
        Debug.Log("Phase1");
        RandomPattern().Forget();
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateMachine.Mouse.CheckPhaseChangeHp() && stateMachine.Mouse.PhaseChange == false)
        {
            stateMachine.SetState("PhaseChange");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        cts.Cancel();
    }
    
    public async UniTaskVoid RandomPattern()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: cts.Token);
        
        if (RandomizerUtil.PercentRandomizer(100))
        {
            Mouse.MovingBackGround?.Invoke(false);
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.Rush()));
            Mouse.MovingBackGround?.Invoke(true);
        }
        else
        {
            stateMachine.ChangeAnimation(EMouseAnimationType.Crying);
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRats()));
        }

        if (RandomizerUtil.PercentRandomizer(stateMachine.Mouse.PatternPercent))
        {
            stateMachine.Mouse.MinusRandomPecent(15f);

            stateMachine.SetState("Serise");

            Debug.Log("레츠고");
        }
        else
        {
            stateMachine.SetState("Run");
        }
    }
}