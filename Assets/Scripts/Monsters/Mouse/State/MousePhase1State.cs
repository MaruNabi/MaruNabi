using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class MousePhase1State : State<MouseStateMachine>
{
    private Transform mouseTransform;
    private float randomPatternPercent;
    private bool isPhase2;

    public MousePhase1State(MouseStateMachine mouseStateMachine) : base(mouseStateMachine)
    {
        randomPatternPercent = 30f;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        RandomPattern().Forget();
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateMachine.Mouse.CheckPhaseChangeHp() && stateMachine.Mouse.PhaseChange == false)
        {
            Debug.Log("PhaseChange");
            stateMachine.SetState("PhaseChange");
        }
    }
    
    public async UniTaskVoid RandomPattern()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        
        if (RandomizerUtil.PercentRandomizer(100))
        {
            Mouse.MovingBackGround?.Invoke(false);
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.TailAttack()));
            Mouse.MovingBackGround?.Invoke(true);
        }
        else
        {
            stateMachine.ChangeAnimation(EMouseAnimationType.Crying);
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRats()));
        }

        // 연속 공격 확률
        if (RandomizerUtil.PercentRandomizer(randomPatternPercent))
        {
            randomPatternPercent -= 15f;

            if (randomPatternPercent <= 10)
                randomPatternPercent = 10f;

            RandomPattern().Forget();
        }
        else
        {
            stateMachine.SetState("Run");
        }
    }
}