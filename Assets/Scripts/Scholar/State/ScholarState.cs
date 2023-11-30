using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarState : State<ScholarStateMachine>
{
    public ScholarState(ScholarStateMachine stateMachine) : base(stateMachine)
    {
    }

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
