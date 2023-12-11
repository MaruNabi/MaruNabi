using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarFan : ScholarState
{
    public ScholarFan(ScholarStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("∫Œ√§");
    }
    public override void OnUpdate()
    {
        base.OnUpdate();

    }
    public override void OnExit()
    {
        base.OnExit();
    }
}
