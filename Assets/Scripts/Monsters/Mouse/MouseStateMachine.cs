using System.Collections.Generic;
using UnityEngine;

public class MouseStateMachine : StateMachine<MouseStateMachine>
{
    public Mouse Mouse => mouse;
    private Mouse mouse;
    private Animator mouseAnimator;

    private void Awake()
    {
        base.states = new Dictionary<string, State<MouseStateMachine>>{
            {"Idle",new MouseIdle(this)},
            {"Appearance",new MouseAppearance(this)},
            {"Fan",new MouseFan(this)},
            {"Leave",new MouseLeave(this)}
        };
    }

    public void Initialize(string _stateName, Mouse _mouse, Animator _mouseAnimator)
    {
        mouse = _mouse;
        mouseAnimator = _mouseAnimator;
        base.Initialize(_stateName);
    }

    public void ChangeAnimation(EAnimationType _type)
    {
        switch (_type)
        {
            case EAnimationType.Attack:
                mouseAnimator.SetTrigger("Attack");
                break;
            case EAnimationType.Hit:
                mouseAnimator.SetTrigger("Hit");
                break;
            case EAnimationType.Laugh:
                mouseAnimator.SetTrigger("Laugh");
                break;
            case EAnimationType.Angry:
                mouseAnimator.SetTrigger("Angry");
                break;
            default:
                break;
        }
    }

    public float GetAnimPlayTime()
    {
        return mouseAnimator.GetCurrentAnimatorStateInfo(0).length;
    }
}