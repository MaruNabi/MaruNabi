using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseState : State<MouseStateMachine>
{
    public MouseState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine)
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
