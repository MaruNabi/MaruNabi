using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class MouseStateMachine : StateMachine<MouseStateMachine>
{
    public Mouse Mouse => mouse;
    private Mouse mouse;
    private Animator mouseAnimator;

    private void Awake()
    {
        base.states = new Dictionary<string, State<MouseStateMachine>>{
            {"Enter",new MouseEnterState(this)},
            {"Run",new MouseRunState(this)},
            {"Phase1",new MousePhase1State(this)},
            {"PhaseChange",new MousePhaseChangeState(this)},
            {"Phase2",new MousePhase2State(this)},
            {"Dead",new MouseDeadState(this)},
        };
    }

    public void Initialize(string _stateName, Mouse _mouse, Animator _mouseAnimator)
    {
        mouse = _mouse;
        mouseAnimator = _mouseAnimator;
        base.Initialize(_stateName);
    }

    public void ChangeAnimation(EMouseAnimationType _type)
    {
        switch (_type)
        {
            case EMouseAnimationType.Run:
                mouseAnimator.SetTrigger("Run");
                break;
            case EMouseAnimationType.Rush:
                mouseAnimator.SetBool("Rush", true);
                break;
            case EMouseAnimationType.NoRush:
                mouseAnimator.SetBool("Rush", false);
                break;
            case EMouseAnimationType.Tail:
                mouseAnimator.SetTrigger("Tail");
                break;
            case EMouseAnimationType.Crying:
                mouseAnimator.SetTrigger("Crying");
                break;
            case EMouseAnimationType.Dead:
                mouseAnimator.SetTrigger("Dead");
                break;
            default:
                break;
        }
    }

    public float GetAnimPlayTime()
    {
        var currentAnimatorStateInfo = mouseAnimator.GetCurrentAnimatorStateInfo(0);
        return currentAnimatorStateInfo.length * currentAnimatorStateInfo.normalizedTime;
    }
}
