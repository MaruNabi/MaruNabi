using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSeriesAttack : State<MouseStateMachine>
{
    public MouseSeriesAttack(MouseStateMachine mouseStateMachine) : base(mouseStateMachine)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        
        if(stateMachine.Mouse.PhaseChange == false)
        {
            if (stateMachine.Mouse.CheckPhaseChangeHp())
            {
                stateMachine.SetState("PhaseChange");
            }
        }
        else
        {
            if (stateMachine.Mouse.CheckDead() && stateMachine.Mouse.Dead == false)
            {
                stateMachine.SetState("Dead");
            }
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("하이");
        if(stateMachine.Mouse.PhaseChange == false)
        {
            stateMachine.SetState("Phase1");
        }
        else
        {
            stateMachine.SetState("Phase2");
        }
    }
}