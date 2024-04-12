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

        if (stateMachine.Mouse.CheckPhaseChangeHp())
        {
            stateMachine.Mouse.SetCanHit(false);
            //stateMachine.Mouse.Phase2 = true;
            //stateMachine.SetState("Phase2");
        }
    }
    
    public async UniTaskVoid RandomPattern()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
        
        if (RandomizerUtil.PercentRandomizer(30))
        {
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.HeadButt()));
        }
        else
        {
            await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRats()));
        }

        // ¿¬¼Ó °ø°Ý È®·ü
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
    
    // public async UniTaskVoid RandomPatternPhase2()
    // {
    //     var takeOne = RandomizerUtil.From(gachaProbability).TakeOne();
    //     switch (takeOne)
    //     {
    //         case EMousePattern.HeadButt:
    //             await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.HeadButt()));
    //             break;
    //         case EMousePattern.SpawnRats:
    //             await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRats()));
    //             break;
    //         case EMousePattern.Rock:
    //             //³«¼®
    //             break;
    //         case EMousePattern.Tail:
    //             //²¿¸®
    //             break;
    //     }
    //
    //     
    //     //¿¬¼Ó °ø°Ý È®·ü
    //     if (RandomizerUtil.PercentRandomizer(randomPatternPercent))
    //     {
    //         randomPatternPercent -= 20f;
    //
    //         if (randomPatternPercent <= 10)
    //             randomPatternPercent = 10f;
    //
    //         RandomPatternPhase2().Forget();
    //     }
    // }
}
