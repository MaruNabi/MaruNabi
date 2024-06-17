using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TigerState : State<TigerStateMachine>
{
    protected CancellationTokenSource cts = new CancellationTokenSource();
    
    public TigerState(TigerStateMachine tigerStateMachine) : base(tigerStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        
        if (stateMachine.tiger.CheckPhase2ChangeHp())
        {
            cts.Cancel();
            stateMachine.tiger.ChangePhase2();
        }
        else if(stateMachine.tiger.CheckPhase3ChangeHp())
        {
            cts.Cancel();
            stateMachine.tiger.ChangePhase3();
        }
        else if (stateMachine.tiger.CheckStageClear())
        {
            cts.Cancel();
            stateMachine.tiger.StageClear();
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