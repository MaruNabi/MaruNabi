using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerState : State<TigerStateMachine>
{
    public TigerState(TigerStateMachine tigerStateMachine) : base(tigerStateMachine) { }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();

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