using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxEnterState : State<FoxStateMachine>
{
    public FoxEnterState(FoxStateMachine foxStateMachine) : base(foxStateMachine) { }

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