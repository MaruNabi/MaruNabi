using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class TigerPhase1State : State<TigerStateMachine>
{
    CancellationTokenSource cts;

    public TigerPhase1State(TigerStateMachine tigerStateMachine) : base(tigerStateMachine)
    {
        cts = new CancellationTokenSource();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Phase1");
        //RandomPattern(cts.Token).Forget();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        // if (stateMachine.Mouse.CheckPhaseChangeHp() && !stateMachine.Mouse.PhaseChange)
        // {
        //     cts.Cancel();
        //     stateMachine.SetState("PhaseChange");
        // }
    }

    // private async UniTask RandomPattern(CancellationToken token)
    // {
    //     try
    //     {
    //         // Cancelled token will throw OperationCanceledException here
    //         await UniTask.Delay(TimeSpan.FromSeconds(0.75f), cancellationToken: token);
    //
    //         if (RandomizerUtil.PercentRandomizer(100))
    //         {
    //             // Check for cancellation before invoking the event
    //             token.ThrowIfCancellationRequested();
    //             
    //             Mouse.MovingBackGround?.Invoke(false);
    //             
    //             // Delay with cancellation token
    //             await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.Rush()), cancellationToken: token);
    //             
    //             // Check for cancellation after the delay
    //             token.ThrowIfCancellationRequested();
    //             
    //             Mouse.MovingBackGround?.Invoke(true);
    //         }
    //         else
    //         {
    //             stateMachine.ChangeAnimation(EMouseAnimationType.Crying);
    //             
    //             // Delay with cancellation token
    //             await UniTask.Delay(TimeSpan.FromSeconds(stateMachine.Mouse.SpawnRats()), cancellationToken: token);
    //         }
    //
    //         // Check for cancellation before proceeding with recursion or state change
    //         token.ThrowIfCancellationRequested();
    //
    //         if (RandomizerUtil.PercentRandomizer(stateMachine.Mouse.PatternPercent))
    //         {
    //             stateMachine.Mouse.MinusRandomPecent(15f);
    //             
    //             // Await the recursive call to properly handle the cancellation token
    //             await RandomPattern(token);
    //         }
    //         else
    //         {
    //             stateMachine.SetState("Run");
    //         }
    //     }
    //     catch (OperationCanceledException)
    //     {
    //         // Handle the cancellation if needed
    //         Debug.Log("RandomPattern cancelled");
    //     }
    // }
}
