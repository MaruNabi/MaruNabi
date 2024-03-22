using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStateMachine : StateMachine<MouseStateMachine>
{
    private Mouse mouse;

    public Mouse Mouse
    {
        get
        {
            return this.mouse;
        }
    }
    private void Awake()
    {
        base.states = new Dictionary<string, State<MouseStateMachine>>{
            {"Idle",new MouseIdle(this)},
            {"Appearance",new MouseAppearance(this)},
            {"Fan",new MouseFan(this)},
        };
    }

    public void Initialize(string stateName, Mouse mouse)
    {
        this.mouse = mouse;
        base.Initialize(stateName);
    }
    
    public void ChangeAnimationAttack()
    {
        mouse.mouseAnimator.SetBool("Attack", true);
    }
}
