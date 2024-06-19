using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MouseState : State<MouseStateMachine>
{
    public CancellationTokenSource cts = new CancellationTokenSource();
    public MouseState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (stateMachine.Mouse.CheckPhaseChangeHp() && !stateMachine.Mouse.PhaseChange)
        {
            cts.Cancel();
            stateMachine.SetState("PhaseChange");
        }
        else if (stateMachine.Mouse.CheckDead() && !stateMachine.Mouse.Dead)
        {
            cts.Cancel();
            stateMachine.SetState("Dead");
            stateMachine.Mouse.OnDead();
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    
    public override void OnExit()
    {
        base.OnExit();
    }
}