using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MouseRunState : State<MouseStateMachine>
{
    private Transform mouseTransform;
    private const float RUNNNING_TIME = 2f;
    
    public MouseRunState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Run");
        RunWait().Forget();
    }
    
    async UniTaskVoid RunWait()
    {        
        if(stateMachine.GetAnimName("Mouse_Run") == false)
        {
            Mouse.MovingBackGround?.Invoke(true);
            stateMachine.ChangeAnimation(EMouseAnimationType.Run);
        }
        
        await UniTask.Delay(TimeSpan.FromSeconds(RUNNNING_TIME));
        
        if(stateMachine.Mouse.PhaseChange)
            stateMachine.SetState("Phase2");
        else
            stateMachine.SetState("Phase1");
    }
}
